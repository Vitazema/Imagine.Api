using AutoMapper;
using Imagine.Auth.Extensions;
using Imagine.Auth.Repository;
using Imagine.Core.Contracts;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AccountController(IUserRepository userRepository,
        IPermissionRepository permissionRepository, IMapper mapper, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userRepository.GetCurrentUserAsync(User);
        var permissions = await _permissionRepository.GetPermissionsAsync(user);
        var userDto = _mapper.Map<User, UserDto>(user);
        userDto.Permission = permissions;
        return Ok(userDto);
    }

    [Authorize]
    [HttpGet("permissions")]
    public async Task<ActionResult<UserPermission>> GetPermissionsAsync()
    {
        var user = await _userRepository.GetCurrentUserAsync(User);
        var permissions = await _permissionRepository.GetPermissionsAsync(user);
        return Ok(permissions);
    }

    [Authorize]
    [HttpGet("settings")]
    public async Task<ActionResult<UserSettings>> GetCurrentUserSettings()
    {
        return await _userRepository.GetCurrentUserSettingsAsync(User);
    }
    
    [Authorize]
    [HttpPut("settings")]
    public async Task<ActionResult<UserSettingsDto>> UpdateCurrentUserSettings(UserSettingsDto userSettings)
    {
        var updatedUserSettings = await _userRepository.UpdateUserSettingsAsync(User, userSettings);
        return Ok(updatedUserSettings);
    }

    [Authorize]
    [HttpGet("subscription")]
    public async Task<ActionResult<Subscription>> GetUserSubscription()
    {
        var user = await _userManager.FindFullUserByClaimsAsync(User);
        return Ok(user?.Subscription);
    }
    
    [Authorize]
    [HttpPut("subscription")]
    public async Task<ActionResult<SubscriptionDto>> UpdateUserSubscription(int addDays, Role role)
    {
        var user = await _userManager.FindFullUserByClaimsAsync(User);
        if (user == null) return BadRequest(new ApiResponse(400));
        if (user.Subscription == null)
        {
            user.Subscription = new Subscription()
            {
                User = user,
                ExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(addDays),
            };
        }
        else
        {
            user.Subscription.ExpiresAt += TimeSpan.FromDays(addDays);
        }
        
        user.Role = user.Role == Role.System ? user.Role : role;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return Ok(_mapper.Map<User, SubscriptionDto>(user));
        return BadRequest(new ApiResponse(400));
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] UserCredentials credentials)
    {
        var user = await _userRepository.Login(credentials);
        if (user == null) return Unauthorized(new ApiResponse(401));

        return Ok(user);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerInfo)
    {
        return await _userRepository.Register(registerInfo);
    }
    
    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userRepository.CheckEmailExistsAsync(email);
    }
    
    [Authorize]
    [HttpPut("updateemail")]
    public async Task<ActionResult<UserDto>> UpdateEmailAsync([FromQuery] string email)
    {
        var user = await _userRepository.UpdateEmailAsync(User, email);
        
        return Ok(user);
    }
}
