using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Imagine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Extensions;

static class DbInitializerExtensions
{
    public static async void DbInitialize(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ArtDbContext>();
        var appSettings = serviceScope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
        if (app.Environment.IsDevelopment())
        {
            await SeedDbAsync(context, appSettings, app.Logger);
        }
        else
        {
            await context.Database.MigrateAsync();
        }
    }

    private static async Task SeedDbAsync(ArtDbContext context, AppSettings appSettings, ILogger logger)
    {
        try
        {
            var artsJson = await File.ReadAllTextAsync(Path.Join(appSettings.ExecutionDirectory,
                appSettings.SeedFilesDirectory, appSettings.ArtsSeedDataFile));
            var arts = JsonSerializer.Deserialize<List<Art>>(artsJson);
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
        };
        await context.SaveChangesAsync();

    }
}
