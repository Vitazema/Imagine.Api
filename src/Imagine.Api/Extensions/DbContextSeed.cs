using Imagine.Core.Configurations;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Imagine.Api.Extensions;

public static class DbContextSeed
{
    private const string UsersSeedDataFile = "Users.json";
    private const string ArtsSeedDataFile = "Arts.json";

    
    public static async Task<List<User>> SeedUsersAsync(UserManager<User> userManager, AppSettings appSettings)
    {
        var usersDataFile = Path.Join(appSettings.ExecutionDirectory, appSettings.SeedFilesDirectory,
            UsersSeedDataFile);
        using var reader = new StreamReader(usersDataFile);
        var usersJson = await reader.ReadToEndAsync();
        var users = JsonSerializer.Deserialize<List<User>>(usersJson);

        foreach (var user in users.Where(user => userManager.FindByNameAsync(user.UserName) == null))
        {
            await userManager.CreateAsync(user);
        }

        return users;
    }
    
    public static async Task SeedDbAsync(ArtDbContext context, AppSettings appSettings, ILogger logger, UserManager<User> userManager)
    {
        try
        {
            var artsJson = await File.ReadAllTextAsync(Path.Join(appSettings.ExecutionDirectory,
                appSettings.SeedFilesDirectory, ArtsSeedDataFile));
            var arts = JsonSerializer.Deserialize<List<Art>>(artsJson);
            var systemUser = await userManager.FindByNameAsync("System"); 
            foreach (var art in arts)
            {
                art.User = systemUser;
            }
            await SeedEntityAsync(arts, context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    private static async Task SeedEntityAsync<T>(ICollection<T> entities, ArtDbContext context) where T : BaseEntity
    {
        foreach (var entity in entities)
        {
            if (!context.Set<T>().Contains(entity))
            {
                context.Set<T>().Add(entity);
            }
        }
        await context.SaveChangesAsync();
    }
}
