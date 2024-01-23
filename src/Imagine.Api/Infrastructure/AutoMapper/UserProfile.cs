using AutoMapper;
using Imagine.Core.Contracts;
using Imagine.Core.Entities.Identity;

namespace Imagine.Api.Infrastructure.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(u => u.Id, o => o.MapFrom(u => u.Id))
            .ForMember(u => u.UserName, o => o.MapFrom(u => u.UserName))
            .ForMember(u => u.Role, o => o.MapFrom(u => u.Role))
            .ForMember(u => u.Subscription, o => o.MapFrom(u =>
                new SubscriptionDto()
                {
                    Role = u.Role,
                    ExpiresAt = u.Subscription.ExpiresAt,
                    UserName = u.UserName
                }))
            .ForMember(u => u.UserSettings, o => o.MapFrom(x => new UserSettingsDto()
            {
                AiType = x.UserSettings.SelectedAiType
            }));

        CreateMap<User, SubscriptionDto>()
            .ForMember(s => s.UserName, o => o.MapFrom(s => s.UserName))
            .ForMember(s => s.ExpiresAt, o => o.MapFrom(s => s.Subscription.ExpiresAt))
            .ForMember(s => s.Role, o => o.MapFrom(s => s.Role));

        CreateMap<UserSettings, UserSettingsDto>()
            .ForMember(u => u.AiType, o => o.MapFrom(u => u.SelectedAiType));
        
        CreateMap<UserSettingsDto, UserSettings>()
            .ForMember(u => u.SelectedAiType, o => o.MapFrom(u => u.AiType));
    }
}
