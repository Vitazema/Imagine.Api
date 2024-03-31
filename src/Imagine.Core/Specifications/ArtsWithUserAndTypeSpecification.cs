using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtsWithUserAndTypeSpecification : BaseSpecification<Art>
{
    public ArtsWithUserAndTypeSpecification(ArtSpecRequest artRequest)
        : base(x =>
            (string.IsNullOrEmpty(artRequest.Search) || x.ArtSetting.ToLower()
                .Contains(artRequest.Search)) &&
            (!artRequest.UserId.HasValue || x.User.Id.ToString() == artRequest.UserId.ToString()) &&
            (!artRequest.ArtType.HasValue || x.Type == artRequest.ArtType)
        )
    {
        AddInclude(x => x.User);
        ApplyPagination(artRequest.PageSize * (artRequest.PageIndex - 1), artRequest.PageSize);
        
        if (!string.IsNullOrEmpty(artRequest.Sort))
        {
            switch (artRequest.Sort)
            {
                case "createdAtAsc":
                    AddOrderBy(a => a.CreatedAt);
                    break;
                case "createdAtDesc":
                    AddOrderByDescending(a => a.CreatedAt);
                    break;
                default:
                    AddOrderBy(a => a.CreatedAt);
                    break;
            }
        }
    }

    public ArtsWithUserAndTypeSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.User);
    }
}
