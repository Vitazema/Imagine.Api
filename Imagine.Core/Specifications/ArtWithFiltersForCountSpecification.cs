using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtWithFiltersForCountSpecification : BaseSpecification<Art>
{
    public ArtWithFiltersForCountSpecification(ArtSpecRequest artRequest) 
        : base(x =>
            (!artRequest.UserId.HasValue || x.User.Id == artRequest.UserId) &&
            (!artRequest.ArtType.HasValue || x.ArtSettings.Type == artRequest.ArtType)
        )
    {
        
    }
}