using AutoMapper;
using Imagine.Api.Configuration;
using Imagine.Api.Dtos;
using Imagine.Core.Entities;

namespace Imagine.Api.Helpers;

public class ArtUrlResolver : IValueResolver<Art, ArtRequestDto, string>
{
    private readonly IConfiguration _config;

    public ArtUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(Art source, ArtRequestDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Url)) return null;
        var environmentUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").FirstOrDefault();
        if (environmentUrl == null) return null;
        var baseUri = new Uri(environmentUrl.Replace("+", "localhost"));
        var urlPath = new Uri(baseUri, source.Url);
        return urlPath.AbsoluteUri;

    }
}
