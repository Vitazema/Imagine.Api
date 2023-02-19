using Microsoft.OpenApi.Models;

namespace Imagine.Api.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o => o.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "API",
            Version = "v1"
        }));
        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        
        app.UseSwagger();
        app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
        
        return app;
    }
}