using Imagine.Core.Entities;
using Imagine.Core.Specifications;

namespace Imagine.Core.Interfaces;

public interface IRepository<T> where T :  BaseEntity
{
    Task<T> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T> GetEntityWithSpec(ISpecification<T> specification);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
    Task<int> CountAsync(ISpecification<T> specification);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<Guid?> DeleteAsync(Guid id);
}