using AutoMapper;
using Imagine.Api.Configuration;
using Imagine.Api.Dtos;
using Imagine.Core.Entities;

namespace Imagine.Api.Helpers;

public class ArtUrlResolver : IValueResolver<Art, ArtResponseDto, string>
{
    private readonly IConfiguration _config;

    public ArtUrlResolver(IConfiguration config)
    {
        _config = config;
    }
    
    public string Resolve(Art source, ArtResponseDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Url)) return null;
        var url = _config.GetSection("Settings").Get<Settings>().AppUrl.Split(";").FirstOrDefault();
        return url + source.Url;

    }
}