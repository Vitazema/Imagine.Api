using Imagine.Api.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Imagine.Api.Infrastructure.AutoMapper;

public class ArtUrlResolver : IValueResolver<Art, ArtDto, List<string>>
{
    public List<string> Resolve(Art source, ArtDto destination, List<string> destMember, ResolutionContext context)
    {
        var urls = new List<string>();
        if (source.Urls.IsNullOrEmpty()) return null;

        foreach (var url in source.Urls)
        {
            var baseUri = new Uri(Urls.BaseUrl);
            var urlPath = new Uri(baseUri, Path.Combine("storage", url));
            urls.Add(urlPath.AbsoluteUri);
        }

        return urls;
    }
}
