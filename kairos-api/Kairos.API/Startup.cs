using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
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
using Newtonsoft.Json;

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
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
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
                _logger.LogInformation("Migration de la base de données si nécessaire");
                context.Database.Migrate();

                var serviceIdDev = "serviceIdDev";
                var emailDev = "dev@kairos.com";

                if (env.IsDevelopment())
                {
                    _logger.LogInformation("Lancement de l'ajout des données fictives dans la base de données");
                    // Si on trouve l'utilisateur, on le supprime. (le reste des éléments devrait aussi se remove étant donné qu'ils sont config en cascade)
                    var devUserExisting = await context.Users.FirstOrDefaultAsync(u => u.ServiceId == serviceIdDev && u.Email == emailDev);
                    var groupsDev = context.Groups.Where(g => g.OwnerId == devUserExisting.UserId).ToList();
                    var eventsDev = context.Groups.Where(g => g.OwnerId == devUserExisting.UserId && g.Event != null).Select(g => g.Event)
                        .ToList();
                    
                    if (eventsDev.Count > 0)
                    {
                        _logger.LogWarning("{} event détecté. Lancement de la suppression", eventsDev.Count);
                        context.Events.RemoveRange(eventsDev);
                    }

                    if (groupsDev.Count > 0)
                    {
                        _logger.LogWarning("{} group détecté. Lancement de la suppression", groupsDev.Count);
                        context.Groups.RemoveRange(groupsDev);
                    }
                    
                    if (devUserExisting != null)
                    {
                        _logger.LogWarning("L'utilisateur (id: {}) de développement détecté, Lancement de sa suppression", devUserExisting.UserId);
                        context.Users.Remove(devUserExisting);
                    }
                    
                    // On crée un user
                    var devUser = new User(serviceIdDev, "Developer", "Best", DateTime.Today, emailDev, DateTime.UtcNow);
                    context.Users.Add(devUser);
                    await context.SaveChangesAsync();
                    
                    // Il affiche le jwt pour qu'on puisse l'utiliser.
                    _logger.LogInformation("New DevUser (id:{})",devUser.UserId);

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
                    _logger.LogInformation("Création de {} nouveau label", labels.Count);

                    // Pour plus tard dans le code
                    var labelsExisting = context.Labels.Where(l => l.UserId == devUser.UserId).ToList();
                    
                    // On crée un groupe privé
                    var devGroupPrivate = new Group("Dev Group Private", devUser.UserId);
                    context.Groups.Add(devGroupPrivate);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'un nouveau groupe privé (id: {})", devGroupPrivate.GroupId);
                    
                    // Ajoute des labels au groupe privé
                    devGroupPrivate.Labels = new List<Label>();
                    labelsExisting.ForEach(l =>
                    {
                        devGroupPrivate.Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr le groupe {} (id: {})", devGroupPrivate.Labels.Count, devGroupPrivate.GroupName, devGroupPrivate.GroupId);

                    // Ajout d'un event au groupe privé
                    Event eventPrivateDev = new Event(DateTime.Today, "Dev Private Event", "Dev Private Event Description");
                    context.Events.Add(eventPrivateDev);                    
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'un nouvel event privé (id: {})", eventPrivateDev.EventId);

                    eventPrivateDev.Labels = new List<Label>();
                    labelsExisting.ForEach(l =>
                    {
                        eventPrivateDev.Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr l'event privé {} (id: {})", eventPrivateDev.Labels.Count, eventPrivateDev.EventTitle, eventPrivateDev.EventId);

                    devGroupPrivate.Event = eventPrivateDev;
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de l'event (id: {}) dans le groupe privé (id: {})", devGroupPrivate.Event.EventId, devGroupPrivate.GroupId);
                    
                    // On crée un groupe publique
                    var devGroupPublic = new Group("Dev Group Public", devUser.UserId);
                    context.Groups.Add(devGroupPublic);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'un nouveau groupe publique (id: {})", devGroupPublic.GroupId);

                    // Ajoute des labels au groupe publique
                    devGroupPublic.Labels = new List<Label>();
                    devGroupPublic.Labels.Add(labelsExisting[2]);
                    devGroupPublic.Labels.Add(labelsExisting[3]);
                    devGroupPublic.Labels.Add(labelsExisting[4]);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr le groupe {} (id: {})", devGroupPublic.Labels.Count, devGroupPublic.GroupName, devGroupPublic.GroupId);

                    // Ajout d'un event au groupe publique
                    Event eventPublicDev = new Event(DateTime.Today, "Dev Public Event", "Dev Public Event Description");
                    context.Events.Add(eventPublicDev);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'un nouvel event publique (id: {})", eventPublicDev.EventId);
                    
                    eventPublicDev.Labels = new List<Label>();
                    eventPublicDev.Labels.Add(labelsExisting[2]);
                    eventPublicDev.Labels.Add(labelsExisting[3]);
                    eventPublicDev.Labels.Add(labelsExisting[4]);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr l'event publique {} (id: {})", eventPublicDev.Labels.Count, eventPublicDev.EventTitle, eventPublicDev.EventId);

                    devGroupPublic.Event = eventPublicDev;
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de l'event (id: {}) dans le groupe publique (id: {})", devGroupPublic.Event.EventId, devGroupPublic.GroupId);
                    
                    // Création d'un studies pour le groupe privé
                    List<Studies> studies = new List<Studies>();
                    studies.Add(new Studies("3482741_prv", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Math", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Physique", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Allemand", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Anglais", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Science", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    studies.Add(new Studies("Révision", GenerateTimeStudiesInSeconds(5, 120), DateTime.Today, devGroupPrivate.GroupId));
                    
                    studies.ForEach(s =>
                    {
                        context.Studies.Add(s);
                    });
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création de {} nouvelles sessions d'étude privé (id: {})", studies.Count, studies[0].StudiesId);

                    studies[0].Labels = new List<Label>();
                    labelsExisting.ForEach(l =>
                    {
                        studies[0].Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr la session d'étude privé {} (id: {})", studies[0].Labels.Count, studies[0].StudiesNumber, studies[0].StudiesId);
                    
                    // Création d'un studies pour le groupe publique
                    Studies studiesPublicDev = new Studies("3482741_pub", "3050", DateTime.Today, devGroupPublic.GroupId);
                    context.Studies.Add(studiesPublicDev);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'une nouvelle session d'étude publique (id: {})", studiesPublicDev.StudiesId);
                    
                    studiesPublicDev.Labels = new List<Label>();
                    labelsExisting.ForEach(l =>
                    {
                        studiesPublicDev.Labels.Add(l);
                    });
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Ajout de {} labels pr la session d'étude publique {} (id: {})", studiesPublicDev.Labels.Count, studiesPublicDev.StudiesNumber, studiesPublicDev.StudiesId);

                    // On crée un companion
                    var devCompanion = new Companion("DevCompanion", devUser.UserId, CompanionType.DOG);
                    context.Companions.Add(devCompanion);
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Création d'un nouveau companion (id: {})", devCompanion.CompanionId);
                    _logger.LogInformation("New DevUser (id:{}) created with the JWT: \n\n{}",devUser.UserId,  JwtUtils.GenerateJsonWebToken(devUser));
                }
            }
        }

        private string GenerateTimeStudiesInSeconds(int minutesDepart, int minutesFin)
        {
            Random random = new Random();
            return (random.Next(minutesDepart, minutesFin) * 60).ToString();
        }
    }
}