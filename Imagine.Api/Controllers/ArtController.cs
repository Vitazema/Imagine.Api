using AutoMapper;
using Imagine.Api.Dtos;
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
    private readonly IMapper _mapper;

    public ArtsController(IRepository<Art> artsRepository,
        IRepository<ArtSettings> artSettingsRepository,
        IMapper mapper
    )
    {
        _artsRepository = artsRepository;
        _artSettingsRepository = artSettingsRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArtResponseDto>>> GetArts()
    {
        var specification = new ArtsWithUserAndSettingSpecification();
        var arts = await _artsRepository.ListAsync(specification);

        return Ok(_mapper.Map<IReadOnlyList<Art>, IReadOnlyList<ArtResponseDto>>(arts));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArtResponseDto>> GetArt(int id)
    {
        var specification = new ArtsWithUserAndSettingSpecification(id);
        var art = await _artsRepository.GetEntityWithSpec(specification);
        
        return Ok(_mapper.Map<Art, ArtResponseDto>(art));
    }

    [HttpGet("settings/{id:int}")]
    public async Task<ActionResult<IReadOnlyList<ArtSettings>>> GetSettings(int id)
    {
        return Ok(await _artSettingsRepository.GetByIdAsync(id));
    }
}