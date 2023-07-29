using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Extensions;

static class DbInitializerExtensions
{
    public static async void DbInitialize(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
        var services = serviceScope.ServiceProvider;
        var artContext = services.GetRequiredService<ArtDbContext>();
        var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;

        var userManager = services.GetRequiredService<UserManager<User>>();
        var identityContext = services.GetRequiredService<UserIdentityDbContext>();


        if (app.Environment.IsDevelopment())
        {
            await identityContext.Database.MigrateAsync();
            await artContext.Database.MigrateAsync();
            var users = await IdentityDbContextSeed.SeedUsersAsync(userManager, appSettings);
            await SeedDbAsync(artContext, appSettings, app.Logger, users.FirstOrDefault());
        }
        else
        {
            await identityContext.Database.MigrateAsync();
            await artContext.Database.MigrateAsync();
            await IdentityDbContextSeed.SeedUsersAsync(userManager, appSettings);
        }
    }

    private static async Task SeedDbAsync(ArtDbContext context, AppSettings appSettings, ILogger logger,
        User systemUser)
    {
        try
        {
            var artsJson = await File.ReadAllTextAsync(Path.Join(appSettings.ExecutionDirectory,
                appSettings.SeedFilesDirectory, appSettings.ArtsSeedDataFile));
            var arts = JsonSerializer.Deserialize<List<Art>>(artsJson);
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

        ;
        await context.SaveChangesAsync();
    }
}
