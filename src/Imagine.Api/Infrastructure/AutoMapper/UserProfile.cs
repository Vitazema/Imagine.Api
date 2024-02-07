namespace Imagine.Api.Infrastructure.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(u => u.Id, o => o.MapFrom(u => u.Id))
            .ForMember(u => u.UserName, o => o.MapFrom(u => u.UserName))
            .ForMember(u => u.Role, o => o.MapFrom(u => u.Role))
            .ForMember(u => u.UserSettings, o => o.MapFrom(x => new UserSettingsDto()
            {
                AiType = x.UserSettings.SelectedAiType
            }));

        CreateMap<User, SubscriptionDto>()
            .ForMember(s => s.UserName, o => o.MapFrom(s => s.UserName))
            .ForMember(s => s.Role, o => o.MapFrom(s => s.Role));

        CreateMap<UserSettings, UserSettingsDto>()
            .ForMember(u => u.AiType, o => o.MapFrom(u => u.SelectedAiType))
            .ForMember(u => u.Language, o => o.MapFrom(u => u.Language));
        
        CreateMap<UserSettingsDto, UserSettings>()
            .ForMember(u => u.SelectedAiType, o => o.MapFrom(u => u.AiType))
            .ForMember(u => u.Language, o => o.MapFrom(u => u.Language));

        CreateMap<Order, OrderDto>();
        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(s => s.UserName, s => s.MapFrom(x => x.User.UserName))
            .ForMember(s => s.Role, s => s.MapFrom(x => x.User.Role));
    }
}
