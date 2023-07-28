using Imagine.Auth.Repository;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;

    public UsersController(IUserRepository userRepository,
        IPermissionRepository permissionRepository)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<User>> GetUserAsync([FromQuery]string userName)
    {
        return Ok(await _userRepository.GetUserAsync(userName));
    }
    
    [HttpGet("permissions")]
    public async Task<ActionResult<UserPermission>> GetPermissionsAsync([FromQuery]string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        var permissions = await _permissionRepository.GetPermissionsAsync(user);
        return Ok(permissions);
    }
}
