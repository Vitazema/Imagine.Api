using Imagine.Core.Entities;
using Imagine.Core.Interfaces;
using Imagine.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Infrastructure.Persistence;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ArtDbContext _context;

    public Repository(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    
    public async Task<T> UpdateAsync(T entity)
    {
        var entityToUpdate = await _context.Set<T>().FindAsync(entity.Id);
        if (entityToUpdate == null) return null;
        
        _context.Entry(entityToUpdate).State = EntityState.Detached;
        entityToUpdate = entity;
        _context.Entry(entityToUpdate).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        var addedEntity = await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return addedEntity.Entity;
    }

    public async Task<Guid?> DeleteAsync(Guid id)
    {
        var existedEntity = await _context.Set<T>().FindAsync(id);
        if (existedEntity == null) return null;

        _context.Set<T>().Remove(existedEntity);
        await _context.SaveChangesAsync();
        return id;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }
}
