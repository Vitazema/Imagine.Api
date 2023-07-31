using Imagine.Api.Errors;
using Imagine.Api.Permissions;
using Imagine.Api.Queue;
using Imagine.Api.Services;
using Imagine.Auth.Repository;
using Imagine.Core.Configurations;
using Imagine.Core.Interfaces;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Extensions;

public static class ApplicationServicesExtensions
{
    // Add services to the container.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient();

        services.AddScoped<IArtStorage, ArtStorage>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddSingleton<ITaskProgressService, TaskProgressService>();
        services.AddScoped<IAiApiService, StableDiffusionApiService>();
        services.AddScoped<IAiService, AiService>();
        // services.AddSingleton<IBackgroundTaskQueue>(_ => new AiBackgroundTaskQueue(
        //     appSettings.QueueCapacity));
        services.AddSingleton<IBackgroundTaskQueue, AiBackgroundTaskQueue>();
        services.AddHostedService<AiServiceQueue>();

        services.AddScoped<IPermissionChecker, PermissionChecker>();
        services.AddScoped(x => new Lazy<IPermissionChecker>(x.GetRequiredService<IPermissionChecker>));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Any())
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();
                var errorResponse = new ApiValidationErrorResponse()
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }
}
