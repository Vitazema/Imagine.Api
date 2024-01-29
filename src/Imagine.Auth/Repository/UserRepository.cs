using System.Net;
using System.Security.Claims;
using AutoMapper;
using Imagine.Auth.Extensions;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Imagine.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Auth.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _singInManager;
    private readonly IMapper _mapper;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ITokenService _tokenService;

    public UserRepository(UserManager<User> userManager,
        SignInManager<User> singInManager, IMapper mapper, IPermissionRepository permissionRepository,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _singInManager = singInManager;
        _mapper = mapper;
        _permissionRepository = permissionRepository;
        _tokenService = tokenService;
    }

    public async Task<User?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.Name);
        if (userName == null) throw new InvalidOperationException("User not found or unauthorized");

        return await _userManager.FindUserByNameWithFullInfoAsync(userName);
    }

    public async Task<User?> GetUserAsync(string? userName)
    {
        if (userName == null)
        {
            return null;
        }

        var user = await _userManager.FindByNameAsync(userName);

        return user;
    }

    public async Task<User?> GetUserByIdAsync(string? userId)
    {
        if (userId == null)
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }

    public async Task<UserDto?> Login(UserCredentials userCredentials)
    {
        //todo: make work with email too
        if (userCredentials.UserInput.IsValidEmailAddress())
            return null;
        var user = await _userManager.FindUserByNameWithFullInfoAsync(userCredentials.UserInput);
        if (user == null) return null;
        if (userCredentials.Password.Length > 0)
        {
            var result = await _singInManager.CheckPasswordSignInAsync(user, userCredentials.Password, false);
            if (!result.Succeeded) return null;
        }

        var userDto = _mapper.Map<User, UserDto>(user);
        userDto.Permission = await _permissionRepository.GetPermissionsAsync(user);
        userDto.Token = _tokenService.CreateToken(user);

        return userDto;
    }

    public async Task<UserDto?> Register(RegisterDto registerInfo)
    {
        var newUser = new User()
        {
            UserName = registerInfo.UserName,
            Email = registerInfo.Email,
            Role = Role.Free
        };

        IdentityResult result;
        
        if (registerInfo.Password.Length > 0)
            result = await _userManager.CreateAsync(newUser, registerInfo.Password);
        else
            result = await _userManager.CreateAsync(newUser);
        
        if (!result.Succeeded) return null;

        var user = _mapper.Map<User, UserDto>(newUser);
        user.Permission = await _permissionRepository.GetPermissionsAsync(newUser);
        user.Token = _tokenService.CreateToken(newUser);
        return user;
    }

    public async Task<UserSettings?> GetCurrentUserSettingsAsync(ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.Name);
        if (userName == null) throw new InvalidOperationException("User not found or unauthorized");

        var currentUser = await _userManager.FindUserByNameWithFullInfoAsync(userName);
        return currentUser?.UserSettings;
    }

    public async Task<UserSettingsDto> UpdateUserSettingsAsync(ClaimsPrincipal userPrincipal,
        UserSettingsDto userSettings)
    {
        var userName = userPrincipal.FindFirstValue(ClaimTypes.Name);
        if (userName == null) throw new InvalidOperationException("User not found or unauthorized");

        for (int retry = 0; retry < 3; retry++)
        {
            try
            {
                var user = await _userManager.FindUserByNameWithFullInfoAsync(userName);
                if (user == null) throw new InvalidOperationException("User not found or unauthorized");

                user.UserSettings ??= new UserSettings()
                {
                    UserId = user.Id,
                    User = user,
                };

                user.UserSettings.SelectedAiType = userSettings.AiType;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return userSettings;
                throw new InvalidOperationException("Can't save user settings for user: " + userName);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (retry == 3) throw;
            }

            await Task.Delay(300);
        }

        throw new InvalidOperationException("Update settings failed after multiple attempts for user: " + userName);
    }
}
