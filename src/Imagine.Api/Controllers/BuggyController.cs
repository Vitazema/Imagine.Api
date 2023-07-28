using Imagine.Api.Errors;
using Imagine.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class BuggyController : BaseApiController
{
    private readonly ArtDbContext _context;

    public BuggyController(ArtDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        var item = _context.Arts.Find(-69);
        
        if (item == null)
            return NotFound(new ApiResponse(404));
        return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerErrorRequest()
    {
        var item = _context.Arts.Find(-69);

        var itemToReturnException = item.Id;
            
        return Ok();
    }
    
    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }
    
    [HttpGet("badrequest/{id}")]
    public ActionResult GetValidatedBadRequest(int id)
    {
        return BadRequest();
    }
}