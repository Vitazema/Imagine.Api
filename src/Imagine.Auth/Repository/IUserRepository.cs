using System.Security.Claims;
using Imagine.Core.Contracts;
using Imagine.Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Auth.Repository;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string? userName);
    Task<User?> GetUserByIdAsync(string? userId);
    Task<UserDto?> Login(UserCredentials credentials);
    Task<ActionResult<UserDto?>> Register(RegisterDto registerInfo);
    Task<User?> GetCurrentUserAsync(ClaimsPrincipal user);
    Task<UserSettings?> GetCurrentUserSettingsAsync(ClaimsPrincipal user);
    Task<UserSettingsDto> UpdateUserSettingsAsync(ClaimsPrincipal userPrincipal, UserSettingsDto newUserSettings);
    Task<bool> CheckEmailExistsAsync(string email);
    Task<bool> CheckUserNameExistsAsync(string userName);
    Task<UserDto> UpdateEmailAsync(ClaimsPrincipal userPrincipal, string newEmail);
}
