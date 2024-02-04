﻿using AutoMapper;
using Imagine.Api.Helpers;
using Imagine.Api.Queue;
using Imagine.Auth.Repository;
using Imagine.Core.Contracts;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Imagine.Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class ArtsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Art> _artsRepository;
    private readonly IUserRepository _usersRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IAiService _aiService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITaskProgressService _taskProgressService;
    private readonly IBackgroundTaskQueue _taskQueue;

    public ArtsController(IUnitOfWork unitOfWork,
        IRepository<Art> artsRepository, IUserRepository usersRepository,
        IPermissionRepository permissionRepository,
        IAiService aiService,
        IMapper mapper, UserManager<User> userManager,
        IServiceScopeFactory serviceScopeFactory, ITaskProgressService taskProgressService,
        IBackgroundTaskQueue taskQueue)
    {
        _unitOfWork = unitOfWork;
        _artsRepository = artsRepository;
        _usersRepository = usersRepository;
        _permissionRepository = permissionRepository;
        _aiService = aiService;
        _mapper = mapper;
        _userManager = userManager;
        _serviceScopeFactory = serviceScopeFactory;
        _taskProgressService = taskProgressService;
        _taskQueue = taskQueue;
    }

    // [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArtDto>>> GetArts([FromQuery] ArtSpecRequest artRequest)
    {
        var specification = new ArtsWithUserAndTypeSpecification(artRequest);

        var countSpecification = new ArtWithFiltersForCountSpecification(artRequest);
        var totalArts = await _artsRepository.CountAsync(countSpecification);

        var arts = await _artsRepository.ListAsync(specification);

        var data = _mapper.Map<IReadOnlyList<ArtDto>>(arts);

        return Ok(new Pagination<ArtDto>(artRequest.PageIndex, artRequest.PageSize,
            totalArts, data));
    }

    // [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtDto>> GetArt(Guid id)
    {
        var specification = new ArtsWithUserAndTypeSpecification(id);
        var art = await _artsRepository.GetEntityWithSpec(specification);

        if (art == null) return NotFound(new ApiResponse(404));

        return Ok(_mapper.Map<Art, ArtDto>(art));
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<ArtDto>> AddArt([FromBody] ArtDto dto)
    {
        var user = await _usersRepository.GetCurrentUserAsync(User);
        if (user == null)
        {
            return BadRequest(new ApiResponse(400, $"Current user not found, please login."));
        }

        var art = await _aiService.CreateArtAsync(user, dto);
        if (art == null)
        {
            return BadRequest(new ApiResponse(400, $"Failed to create art: {dto.Id}"));
        }
        
        var artDto = _mapper.Map<Art, ArtDto>(art);

        return Created($"/gallery/{artDto.Id}", artDto);
    }

    // [Authorize]
    [HttpPut]
    public async Task<ActionResult<ArtDto>> UpdateArt([FromBody] ArtDto dto)
    {
        var art = _mapper.Map<ArtDto, Art>(dto);
        var updatedArt = await _artsRepository.UpdateAsync(art);

        if (updatedArt == null)
        {
            return NotFound(new ApiResponse(404, $"Art {dto.Id} not found"));
        }

        if (updatedArt == null)
            throw new ArgumentNullException(nameof(updatedArt), $"Art with id: {art.Id} cannot be updated");

        return Ok(updatedArt);
    }

    // [Authorize]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<ArtDto>> DeleteArt(Guid id)
    {
        var art = await _artsRepository.GetByIdAsync(id);
        if (art == null)
        {
            return NotFound(new ApiResponse(404, $"Art {id} not found"));
        }

        var result = await _artsRepository.DeleteAsync(art.Id);
        if (result == null)
        {
            return BadRequest(new ApiResponse(400, $"No art with {id}"));
        }

        return Ok();
    }
}
