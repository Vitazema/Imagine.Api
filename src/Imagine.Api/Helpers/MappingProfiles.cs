using AutoMapper;
using Imagine.Api.Dtos;
using Imagine.Core.Entities;

namespace Imagine.Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Art, ArtRequestDto>()
            .ForMember(a => a.User, o => o.MapFrom(u => u.User.FullName))
            .ForMember(a => a.Url, o => o.MapFrom<ArtUrlResolver>());
    }
}
