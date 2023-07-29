using System.Text;
using Imagine.Auth.Services;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Imagine.Api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services.AddIdentityCore<User>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<UserIdentityDbContext>();
        builder.AddSignInManager<SignInManager<User>>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // false == anonimus validations
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"] ?? throw new InvalidOperationException())),
                    
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });
        return services;
    }
}
