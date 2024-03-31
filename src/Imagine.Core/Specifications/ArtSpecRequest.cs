using Imagine.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Core.Specifications;

public record ArtSpecRequest
{
    private const int MaxPageSize = 50;
    [FromQuery(Name = "page")]
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;
    private string _search;
    
    [FromQuery(Name ="limit")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public ArtType? ArtType { get; set; }
    public int? UserId { get; set; }
    public string Sort { get; set; }

    public string Search
    {
        get => _search;
        set => _search = value?.ToLower();
    }
}
