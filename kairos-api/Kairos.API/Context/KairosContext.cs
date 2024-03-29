using System.IO;
using Kairos.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kairos.API.Context;

public class KairosContext : DbContext
{
    protected KairosContext()
    {
    }

    // Lors de la configuration du contexte, cette méthode est appelée pour définir les options de base de données pour ce contexte.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            options.UseNpgsql(configuration.GetConnectionString("KairosDb"));
        }
    }

    public KairosContext(DbContextOptions<KairosContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OAuth2Credentials> OAuth2Credentials { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<Studies> Studies { get; set; }
    public DbSet<Companion> Companions { get; set; }
    public DbSet<Item> Items { get; set; }
}