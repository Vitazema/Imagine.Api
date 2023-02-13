using System.Text.Json;
using Imagine.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Imagine.Infrastructure.Data;

public class ArtDbContextSeed
{
    public static async Task SeedAsync(ArtDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            var systemUser = new User()
            {
                FullName = "System"
            };
            if (!context.Users.Any())
            {
                context.Users.Add(systemUser);
                await context.SaveChangesAsync();
            }

            if (!context.Arts.Any())
            {
                var settingsJson = File.ReadAllText("../Imagine.Infrastructure/Data/SeedData/Settings.json");
                var settings = JsonSerializer.Deserialize<List<ArtSettings>>(settingsJson);

                var artsJson = File.ReadAllText("../Imagine.Infrastructure/Data/SeedData/Arts.json");
                var arts = JsonSerializer.Deserialize<List<Art>>(artsJson);

                foreach (var art in arts.Where(art => !context.Arts.Contains(art)))
                {
                    art.ArtSettings = settings.FirstOrDefault();
                    art.User = systemUser;
                    context.Arts.Add(art);
                }

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<ArtDbContextSeed>();
            logger.LogError(e.Message);
        }
    }
}