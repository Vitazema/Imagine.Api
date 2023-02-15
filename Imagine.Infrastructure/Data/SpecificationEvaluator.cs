using Imagine.Core.Entities;
using Imagine.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Infrastructure.Data;

public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.Includes.Aggregate(query, (current, include) =>
            current.Include(include));
        
        return query;
    }
}