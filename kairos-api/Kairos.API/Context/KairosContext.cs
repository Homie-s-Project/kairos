using System;
using Kairos.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kairos.API.Context;

public class KairosContext: DbContext
{

    protected readonly IConfiguration _configuration;

    protected KairosContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }
        
        // options.UseNpgsql(_configuration.GetConnectionString("KairosDb"));
        Console.WriteLine("Heure");
        options.UseNpgsql("Host=localhost;Port=5432;Database=Kairos;Username=kairos_user;Password=kairos_password;");
    }

    public KairosContext(DbContextOptions<KairosContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<OAuth2Credentials> OAuth2Credentials { get; set; }
}