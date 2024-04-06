using Imagine.Api.Permissions;
using Imagine.Api.Queue;
using Imagine.Api.Services;
using Imagine.Auth.Repository;
using Imagine.Infrastructure.Persistence;

namespace Imagine.Api.Extensions;

public static class AppServicesExtensions
{
    // Add services to the container.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        
        services.AddScoped<IArtStorage, ArtStorage>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddSingleton<ITaskProgressService, TaskProgressService>();
        services.AddSingleton<IWorkerPool, WorkerPool>();
        services.AddScoped<IAiApiService, StableDiffusionQueueApiService>();
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
