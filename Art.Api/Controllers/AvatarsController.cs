using Art.Data;
using Art.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NftArt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvatarsController : ControllerBase
{
    private readonly ArtDbContext _context;

    public AvatarsController(ArtDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<Avatar>>> GetAvatars()
    {
        var avatars = await _context.Avatars.ToListAsync();
        return Ok(avatars);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Avatar>> GetAvatar(int id)
    {
        return await _context.Avatars.FindAsync(id);
    }
}