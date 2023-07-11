using AutoMapper;
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
    private readonly IMapper _mapper;

    public ArtsController(IRepository<Art> artsRepository,
        IMapper mapper
    )
    {
        _artsRepository = artsRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArtDto>>> GetArts([FromQuery] ArtSpecRequest artRequest)
    {
        var specification = new ArtsWithUserAndTypeSpecification(artRequest);

        var countSpecification = new ArtWithFiltersForCountSpecification(artRequest);
        var totalArts = await _artsRepository.CountAsync(countSpecification);
        
        var arts = await _artsRepository.ListAsync(specification);

        var data = _mapper.Map<IReadOnlyList<ArtDto>>(arts);
        
        Thread.Sleep(2000);
        
        return Ok(new Pagination<ArtDto>(artRequest.PageIndex, artRequest.PageSize,
            totalArts, data));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtDto>> GetArt(int id)
    {
        var specification = new ArtsWithUserAndTypeSpecification(id);
        var art = await _artsRepository.GetEntityWithSpec(specification);

        if (art == null) return NotFound(new ApiResponse(404));
        
        return Ok(_mapper.Map<Art, ArtDto>(art));
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> AddArt([FromBody] ArtDto addArt)
    {
        return Ok(Random.Shared.Next(1, 999999999));
    }
}
