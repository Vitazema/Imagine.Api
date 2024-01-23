using System.Security.Claims;
using Imagine.Core.Contracts;
using Imagine.Core.Entities.Identity;

namespace Imagine.Auth.Repository;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string? userName);
    Task<User?> GetUserByIdAsync(string? userId);
    Task<UserDto?> Login(string userName);
    Task<UserDto?> RegisterUser(string userName);
    Task<User?> GetCurrentUserAsync(ClaimsPrincipal user);
    Task<UserSettings?> GetCurrentUserSettingsAsync(ClaimsPrincipal user);
    Task<UserSettingsDto> UpdateUserSettingsAsync(ClaimsPrincipal userPrincipal, UserSettingsDto userSettings);
}
