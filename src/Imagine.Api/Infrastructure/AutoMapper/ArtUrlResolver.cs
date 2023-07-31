using AutoMapper;
using Imagine.Api.Errors;
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
            if (environmentUrl == null) throw new Exception("Can't find application URL");
            var urlStoragePath = environmentUrl.Replace("+", "localhost");
            var baseUri = new Uri(urlStoragePath);
            var urlPath = new Uri(baseUri, Path.Combine("storage", url));
            urls.Add(urlPath.AbsoluteUri);
        }

        return urls;
    }
}
