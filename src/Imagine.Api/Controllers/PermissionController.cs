using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class PermissionController : BaseApiController
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionController(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserPermissions>> GetPermissionsAsync(string id)
    {
        var permissions = await _permissionRepository.GetPermissionsAsync(id);
        return Ok(permissions);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserPermissions>> UpsertPermissionsAsync(UserPermissions userPermissions)
    {
        var permissions = await _permissionRepository.UpsertPermissionsAsync(userPermissions);
        return Ok(permissions);
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> DeletePermissionsAsync(string id)
    {
        return await _permissionRepository.DeletePermissionsAsync(id);
    }
}
