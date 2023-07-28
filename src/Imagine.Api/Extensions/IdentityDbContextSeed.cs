using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Imagine.Api.Extensions;

public static class IdentityDbContextSeed
{
    private const string UsersSeedDataFile = "Users.json";
    
    public static async Task<List<User>> SeedUsersAsync(UserManager<User> userManager, AppSettings appSettings)
    {
        var usersDataFile = Path.Join(appSettings.ExecutionDirectory, appSettings.SeedFilesDirectory,
            UsersSeedDataFile);
        using var reader = new StreamReader(usersDataFile);
        var usersJson = reader.ReadToEnd();
        var users = JsonSerializer.Deserialize<List<User>>(usersJson);

        foreach (var user in users)
        {
            if (!userManager.Users.Contains(user))
            {
                await userManager.CreateAsync(user);
            }
        }

        return users;
    }
}
