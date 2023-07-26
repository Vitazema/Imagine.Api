using Imagine.Api.Errors;
using Imagine.Api.Queue;
using Imagine.Api.Services;
using Imagine.Core.Configurations;
using Imagine.Core.Contracts;
using Imagine.Infrastructure;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Extensions;

public static class ApplicationServicesExtensions
{
    // Add services to the container.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddHttpClient();
        
        services.AddScoped<IArtRepository, ArtRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddSingleton<ITaskProgressService, TaskProgressService>();
        services.AddScoped<IAiApiService, StableDiffusionApiService>();
        services.AddScoped<AiService>();
        services.AddHostedService<AiServiceQueue>();
        services.AddSingleton<IBackgroundTaskQueue>(_ => new AiBackgroundTaskQueue(
            appSettings.QueueCapacity));

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
