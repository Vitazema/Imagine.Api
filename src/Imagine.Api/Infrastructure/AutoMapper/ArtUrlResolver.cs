using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Imagine.Api.Infrastructure.AutoMapper;

public class ArtUrlResolver : IValueResolver<Art, ArtDto, List<string>>
{
    public List<string> Resolve(Art source, ArtDto destination, List<string> destMember, ResolutionContext context)
    {
        var urls = new List<string>();
        if (source.Urls == null) return null;
        foreach (var url in source.Urls)
        {
            if (string.IsNullOrEmpty(url)) return null;
            var environmentUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").FirstOrDefault();
            if (environmentUrl == null) return null;
            var baseUri = new Uri(environmentUrl.Replace("+", "localhost"));
            var urlPath = new Uri(baseUri, url);
            urls.Add(urlPath.AbsoluteUri);
        }

        return urls;
    }
}
