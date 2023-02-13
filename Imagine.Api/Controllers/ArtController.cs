using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtsController : ControllerBase
{
    private readonly IArtRepository _repository;

    public ArtsController(IArtRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Art>>> GetArts()
    {
        return Ok(await _repository.GetArtAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Art>> GetArts(int id)
    {
        return await _repository.GetArtByIdAsync(id);
    }

    [HttpGet("settings/{id:int}")]
    public async Task<ActionResult<List<ArtSettings>>> GetSettings(int id)
    {
        return Ok(await _repository.GetArtSettingsByIdAsync(id));
    }
}