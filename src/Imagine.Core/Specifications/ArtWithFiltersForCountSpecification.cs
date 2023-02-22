using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtWithFiltersForCountSpecification : BaseSpecification<Art>
{
    public ArtWithFiltersForCountSpecification(ArtSpecRequest artRequest) 
        : base(x =>
            (string.IsNullOrEmpty(artRequest.Search) || x.ArtSetting.Prompt.ToLower()
                .Contains(artRequest.Search)) &&
            (!artRequest.UserId.HasValue || x.User.Id == artRequest.UserId) &&
            (!artRequest.ArtType.HasValue || x.ArtSetting.Type == artRequest.ArtType)
        )
    {
        
    }
}