using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace Imagine.Infrastructure.Data.AutoMapper;

public class ArtUrlResolver : IValueResolver<Art, ArtDto, string>
{
    public string Resolve(Art source, ArtDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Url)) return null;
        var environmentUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").FirstOrDefault();
        if (environmentUrl == null) return null;
        var baseUri = new Uri(environmentUrl.Replace("+", "localhost"));
        var urlPath = new Uri(baseUri, source.Url);
        return urlPath.AbsoluteUri;

    }
}
