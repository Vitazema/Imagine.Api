using System.Collections;
using Imagine.Core.Entities;
using Imagine.Core.Interfaces;

namespace Imagine.Infrastructure.Persistence;

public class UnitOfWork(ArtDbContext context) : IUnitOfWork
{
    private Hashtable _repositories;
    public async Task<int> Complete()
    {
        return await context.SaveChangesAsync();
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(
                typeof(TEntity)), context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
