using Imagine.Core.Entities.Identity;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Imagine.Api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<UserIdentityDbContext>();
        builder.AddSignInManager<SignInManager<User>>();

        services.AddAuthentication();
        return services;
    }
}
