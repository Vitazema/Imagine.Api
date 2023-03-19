using AutoMapper;
using Imagine.Api.Dtos;
using Imagine.Api.Errors;
using Imagine.Api.Helpers;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class ArtsController : BaseApiController
{
    private readonly IRepository<Art> _artsRepository;
    private readonly IRepository<ArtSetting> _artSettingsRepository;
    private readonly IMapper _mapper;

    public ArtsController(IRepository<Art> artsRepository,
        IRepository<ArtSetting> artSettingsRepository,
        IMapper mapper
    )
    {
        _artsRepository = artsRepository;
        _artSettingsRepository = artSettingsRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArtRequestDto>>> GetArts([FromQuery] ArtSpecRequest artRequest)
    {
        var specification = new ArtsWithUserAndTypeSpecification(artRequest);

        var countSpecification = new ArtWithFiltersForCountSpecification(artRequest);
        var totalArts = await _artsRepository.CountAsync(countSpecification);
        
        var arts = await _artsRepository.ListAsync(specification);

        var data = _mapper.Map<IReadOnlyList<Art>, IReadOnlyList<ArtRequestDto>>(arts);
        
        return Ok(new Pagination<ArtRequestDto>(artRequest.PageIndex, artRequest.PageSize,
            totalArts, data));
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddArt([FromBody] ArtRequestDto addArtRequest)
    {
        return Ok(Random.Shared.Next(1, 999999999));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtRequestDto>> GetArt(int id)
    {
        var specification = new ArtsWithUserAndTypeSpecification(id);
        var art = await _artsRepository.GetEntityWithSpec(specification);

        if (art == null) return NotFound(new ApiResponse(404));
        
        return Ok(_mapper.Map<Art, ArtRequestDto>(art));
    }

    [HttpGet("settings/{id:int}")]
    public async Task<ActionResult<IReadOnlyList<ArtSetting>>> GetSettings(int id)
    {
        return Ok(await _artSettingsRepository.GetByIdAsync(id));
    }
}
