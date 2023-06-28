using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Imagine.Infrastructure.Data.AutoMapper;

public static class AutoMapperServiceCollectionExtensions
{
    public static void AddCustomAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton<Profile>(new ArtProfile());
        services.AddSingleton(provider => CreateMapperConfiguration(provider).CreateMapper());
    }
    
    private static MapperConfiguration CreateMapperConfiguration(IServiceProvider provider)
    {
        return new MapperConfiguration(c =>
        {
            foreach (var profile in provider.GetServices<Profile>())
            {
                c.AddProfile(profile);
            }
        });
    }
}