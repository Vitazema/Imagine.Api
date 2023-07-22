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
    private readonly IRepository<User> _usersRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;

    public ArtsController(IRepository<Art> artsRepository, IRepository<User> usersRepository,
        IPermissionRepository permissionRepository,
        IMapper mapper
    )
    {
        this._artsRepository = artsRepository;
        _usersRepository = usersRepository;
        _permissionRepository = permissionRepository;
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
        
        Thread.Sleep(1000);
        
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
    public async Task<ActionResult<ArtDto>> AddArt([FromBody] ArtDto dto)
    {
        
        var user = _usersRepository
            .ListAllAsync()
            .Result
            .FirstOrDefault(x => x.FullName == dto.User);
        if (user == null)
        {
            return BadRequest(new ApiResponse(400, $"User {dto.User} not found"));
        }

        var newArt = new Art()
        {
            User = user,
            ArtSetting = dto.ArtSetting.ToJsonString(),
            Title = dto.Title
        };
        
        var userPermission = await _permissionRepository.GetPermissionsAsync(user.FullName);
        var permission = userPermission?.Permissions.FirstOrDefault();
        if (permission != null) permission.Credentials -= 10;
        await _permissionRepository.UpsertPermissionsAsync(userPermission);
        
        // todo: immitating generation 
        Thread.Sleep(2000);

        var artResult = await _artsRepository.AddAsync(newArt);
        var artDto = _mapper.Map<Art, ArtDto>(artResult);
        return Created($"/gallery/{artDto.Id}", artDto);
    }

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
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ArtDto>> DeleteArt(int id)
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
