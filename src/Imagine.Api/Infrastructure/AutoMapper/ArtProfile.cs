using System.Text.Json.Nodes;
using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Infrastructure.AutoMapper;

public sealed class ArtProfile : Profile
{
    public ArtProfile()
    {
        CreateMap<Art, ArtDto>()
            .ForMember(a => a.Id, o => o.MapFrom(u => u.Id))
            .ForMember(a => a.User, o => o.MapFrom(u => u.User.UserName))
            .ForMember(a => a.Urls, o => o.MapFrom<ArtUrlResolver>())
            .ForMember(a => a.ArtSetting, o => o.MapFrom(a => StringToJson.SerializeArtSetting(a.ArtSetting)));
    }
}

public class StringToJson // Replace with your actual class name
{
    public static JsonNode SerializeArtSetting(string artSetting)
    {
        try
        {
        	return JsonNode.Parse(artSetting);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
