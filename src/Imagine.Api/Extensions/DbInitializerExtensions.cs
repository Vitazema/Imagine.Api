using Imagine.Core.Configurations;
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

        if (app.Environment.IsDevelopment())
        {
            await artContext.Database.MigrateAsync();
            var users = await DbContextSeed.SeedUsersAsync(userManager, appSettings);
            await DbContextSeed.SeedDbAsync(artContext, appSettings, app.Logger, users.FirstOrDefault());
        }
        else
        {
            await artContext.Database.MigrateAsync();
            await DbContextSeed.SeedUsersAsync(userManager, appSettings);
        }
    }


}
