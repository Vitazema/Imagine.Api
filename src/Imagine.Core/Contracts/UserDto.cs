using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public UserPermission Permission { get; set; }
    public SubscriptionDto Subscription { get; set; }
    public UserSettingsDto UserSettings { get; set; }
    public Role Role { get; set; }
}
