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
        if (source.Urls.IsNullOrEmpty()) return null;
        
        var environmentUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
            ?.Split(";")
            .FirstOrDefault()
            ?.Replace("+", "localhost");
        
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            environmentUrl = Environment.GetEnvironmentVariable("API_WAN_URL");

        if (environmentUrl.IsNullOrEmpty() || environmentUrl == null) throw new Exception("Can't find application URL");
        
        foreach (var url in source.Urls)
        {
            var baseUri = new Uri(environmentUrl);
            var urlPath = new Uri(baseUri, Path.Combine("storage", url));
            urls.Add(urlPath.AbsoluteUri);
        }

        return urls;
    }
}
