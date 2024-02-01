using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public UserPermission Permission { get; set; }
    public IReadOnlyCollection<SubscriptionDto> Subscriptions { get; set; }
    public UserSettingsDto UserSettings { get; set; }
    public Role Role { get; set; }
}
