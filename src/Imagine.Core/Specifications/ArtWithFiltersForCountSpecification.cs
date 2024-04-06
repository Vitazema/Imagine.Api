using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtWithFiltersForCountSpecification : BaseSpecification<Art>
{
    public ArtWithFiltersForCountSpecification(ArtSpecRequest artRequest) 
        : base(x =>
            (string.IsNullOrEmpty(artRequest.Search) || x.ArtSetting.ToLower()
                .Contains(artRequest.Search)) &&
            (artRequest.User.UserName == "System" || x.User.Id == artRequest.User.Id) &&
            (!artRequest.ArtType.HasValue || x.Type == artRequest.ArtType)
        )
    {
        
    }
}
