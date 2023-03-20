using System.Text.Json;
using Imagine.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Imagine.Infrastructure.Data;

public class ArtDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        try
        {
            await using var context = serviceProvider.GetRequiredService<ArtDbContext>();
            var users = new List<User>()
            {
                new()
                {
                    Id = 1,
                    FullName = "System"
                },
                new()
                {
                    Id = 2,
                    FullName = "UserName"
                }
            };

            var artsJson = File.ReadAllText("../Imagine.Infrastructure/Data/SeedData/Arts.json");
            var arts = JsonSerializer.Deserialize<List<Art>>(artsJson);

            var settingsJson = File.ReadAllText("../Imagine.Infrastructure/Data/SeedData/Settings.json");
            var settings = JsonSerializer.Deserialize<List<ArtSetting>>(settingsJson);
                

            SeedEntity(users, context);
            SeedEntity(settings, context);

            foreach (var art in arts)
            {
                art.ArtSetting = context.ArtSettings.Find(art.ArtSettingId);
            }
                
            SeedEntity(arts, context);

        }
        catch (Exception ex)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<ArtDbContextSeed>();
            logger.LogError(ex.Message);
        }
    }

    private static void SeedEntity<T>(ICollection<T> entities, ArtDbContext context) where T : BaseEntity
    {
        foreach (var entity in entities)
        {
            if (!context.Set<T>().Contains(entity))
                context.Set<T>().Add(entity);
            else
                context.Set<T>().Update(entity);
        };

        context.SaveChanges();
    }
}
