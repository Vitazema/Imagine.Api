using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Infrastructure.Data.AutoMapper;

public class ArtProfile : Profile
{
    internal ArtProfile()
    {
        CreateMap<Art, ArtDto>()
            .ForMember(a => a.Id, o => o.MapFrom(u => u.Id))
            .ForMember(a => a.User, o => o.MapFrom(u => u.User.FullName))
            .ForMember(a => a.Url, o => o.MapFrom<ArtUrlResolver>());
    }
}
