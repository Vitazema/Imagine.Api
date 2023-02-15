using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtsController : ControllerBase
{
    private readonly IRepository<Art> _artsRepository;
    private readonly IRepository<ArtSettings> _artSettingsRepository;

    public ArtsController(IRepository<Art> artsRepository,
        IRepository<ArtSettings> artSettingsRepository
    )
    {
        _artsRepository = artsRepository;
        _artSettingsRepository = artSettingsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Art>>> GetArts()
    {
        var specification = new ArtsWithUserAndSettingSpecification();
        return Ok(await _artsRepository.ListAsync(specification));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Art>> GetArt(int id)
    {
        var specification = new ArtsWithUserAndSettingSpecification(id);
        return await _artsRepository.GetEntityWithSpec(specification);
    }

    [HttpGet("settings/{id:int}")]
    public async Task<ActionResult<List<ArtSettings>>> GetSettings(int id)
    {
        return Ok(await _artSettingsRepository.GetByIdAsync(id));
    }
}