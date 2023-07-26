using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Infrastructure;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class UsersController : BaseApiController
{
    private readonly ArtDbContext _artContext;
    private readonly IPermissionRepository _permissionRepository;

    public UsersController(ArtDbContext artContext, IPermissionRepository permissionRepository)
    {
        _artContext = artContext;
        _permissionRepository = permissionRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<User>> GetUserAsync([FromQuery]string id)
    {
        var isParsed = int.TryParse(id, out var intId);
        if (!isParsed) return BadRequest("Id must be an integer");
        var user = await _artContext.Users.FindAsync(intId);
        return Ok(user);
    }
    
}
