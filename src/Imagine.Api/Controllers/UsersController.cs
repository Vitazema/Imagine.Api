using AutoMapper;
using Imagine.Api.Errors;
using Imagine.Auth.Extensions;
using Imagine.Auth.Repository;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UsersController(IUserRepository userRepository,
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
    [HttpGet("subscription")]
    public async Task<ActionResult<Subscription>> GetUserSubscription()
    {
        var user = await _userManager.FindUserByClaimsPrincipleWithSubscriptionAsync(User);
        return Ok(user?.Subscription);
    }
    
    [Authorize]
    [HttpPut("subscription")]
    public async Task<ActionResult<SubscriptionDto>> UpdateUserSubscription(int addDays, Role role)
    {
        var user = await _userManager.FindUserByClaimsPrincipleWithSubscriptionAsync(User);
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
    public async Task<ActionResult<UserDto>> Login(string userName)
    {
        var user = await _userRepository.Login(userName);

        return Ok(user);
    }
    
    // [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(string userName)
    {
        var user = await _userRepository.RegisterUser(userName);
        if (user == null) return BadRequest(new ApiResponse(400));

        return Ok(user);
    }
}
