using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Controllers;
using Kairos.API.Middleware;
using Kairos.API.Models;
using Kairos.API.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Kairos.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }
        private readonly ILogger<Startup> _logger;
        private readonly DbContextOptions<KairosContext> _contextOptions;
        private readonly IServiceProvider _serviceProvider;
        private const string PolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            _logger = logger;

            _contextOptions = new DbContextOptions<KairosContext>();
            _serviceProvider = serviceProvider;

            Configuration = configuration;
            StaticConfig = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS Policy Config
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, builder =>
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Kairos API", Version = "v1"});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);*/
            });
            
            var connectionString = Configuration.GetConnectionString("KairosDb");
            services.AddDbContext<KairosContext>(opts => opts.UseNpgsql(connectionString));

            // Ajout la gestion d'un cache en mémoire
            services.AddMemoryCache();

            // évite la boucle infinie lors de la sérialisation des objets
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* FIX
             *   InvalidCastException: Cannot write DateTime with kind 
             */
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kairos.API v1"));
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();

            // Config CORS
            app.UseCors(PolicyName);

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Normale qu'il n'y aille pas de await
#pragma warning disable CS4014
            CreateSeed(_serviceProvider, env);
#pragma warning restore CS4014
        }

        /// <summary>
        /// Lorsque l'environement est en DEV, il crée un utilisateur de dev avec des données.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="env"></param>
        private async Task CreateSeed(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            using (var context = new KairosContext(_contextOptions))
            {
                context.Database.Migrate();

                var serviceIdDev = "serviceIdDev";
                var emailDev = "dev@kairos.com";

                if (env.IsDevelopment())
                {
                    // Si on trouve l'utilisateur, on le supprime. (le reste des éléments devrait aussi se remove étant donné qu'ils sont config en cascade)
                    var devUserExisting = await context.Users.FirstOrDefaultAsync(u => u.ServiceId == serviceIdDev && u.Email == emailDev);
                    var groupsDev = context.Groups.Where(g => g.OwnerId == devUserExisting.UserId).ToList();

                    if (groupsDev.Count > 0)
                    {
                        context.Groups.RemoveRange(groupsDev);
                    }
                    
                    if (devUserExisting != null)
                    {
                        context.Users.Remove(devUserExisting);
                    }
                    
                    // On crée un user
                    var devUser = new User(serviceIdDev, "Developer", "Best", DateTime.Today, emailDev, DateTime.UtcNow);
                    context.Users.Add(devUser);
                    await context.SaveChangesAsync();

                    // Il affiche le jwt pour qu'on puisse l'utiliser.
                    _logger.LogWarning("New DevUser (id:{}) created with the JWT: \n\n{}",devUser.UserId,  JwtUtils.GenerateJsonWebToken(devUser));

                    // On crée des labels
                    List<Label> labels = new List<Label>();
                    labels.Add(new Label("Math", devUser.UserId));
                    labels.Add(new Label("Physique", devUser.UserId));
                    labels.Add(new Label("Allemand", devUser.UserId));
                    labels.Add(new Label("Anglais", devUser.UserId));
                    labels.Add(new Label("Science", devUser.UserId));
                    
                    labels.ForEach(l =>
                    {
                        context.Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    // On crée un groupe privé
                    var devGroupPrivate = new Group("Dev Group Private", devUser.UserId);
                    context.Groups.Add(devGroupPrivate);
                    await context.SaveChangesAsync();
                    
                    // On crée un groupe publique
                    var devGroupPublic = new Group("Dev Group Public", devUser.UserId);
                    context.Groups.Add(devGroupPublic);
                    await context.SaveChangesAsync();

                    // Ajoute des labels au groupe privé
                    var labelsExisting = context.Labels.Where(l => l.UserId == devUser.UserId).ToList();

                    devGroupPrivate.Labels = new List<Label>();
                    labelsExisting.ForEach(l =>
                    {
                        devGroupPrivate.Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    // Ajoute des labels au groupe publique
                    devGroupPublic.Labels = new List<Label>();
                    devGroupPublic.Labels.Add(labelsExisting[2]);
                    devGroupPublic.Labels.Add(labelsExisting[3]);
                    devGroupPublic.Labels.Add(labelsExisting[4]);
                    await context.SaveChangesAsync();

                    // On crée un companion
                    var devCompanion = new Companion("DevCompanion", devUser.UserId, CompanionType.DOG);
                    context.Companions.Add(devCompanion);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}