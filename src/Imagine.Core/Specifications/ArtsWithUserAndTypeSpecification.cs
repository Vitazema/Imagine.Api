﻿using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtsWithUserAndTypeSpecification : BaseSpecification<Art>
{
    public ArtsWithUserAndTypeSpecification(ArtSpecRequest artRequest)
        : base(x =>
            (string.IsNullOrEmpty(artRequest.Search) || x.Prompt.ToLower()
                .Contains(artRequest.Search)) &&
            (!artRequest.UserId.HasValue || x.User.Id == artRequest.UserId) &&
            (!artRequest.ArtType.HasValue || x.Type == artRequest.ArtType)
        )
    {
        AddInclude(x => x.User);
        AddOrderBy(x => x.CreatedAt);
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

    public ArtsWithUserAndTypeSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.User);
    }
}