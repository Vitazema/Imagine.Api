using Imagine.Api.Errors;
using Imagine.Core.Contracts;
using Imagine.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Extensions;

public static class ApplicationServicesExtensions
{
    // Add services to the container.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IArtRepository, ArtRepository>();

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