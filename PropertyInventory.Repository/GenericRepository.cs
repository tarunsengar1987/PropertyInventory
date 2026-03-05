using PropertyInventory.Data;
using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Repository;

/// <summary>
/// Generic repository implementation for entities with Guid key (SOLID - Open/Closed).
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly PropertyInventoryDbContext Context;

    protected GenericRepository(PropertyInventoryDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .FindAsync([id], cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        Context.Set<TEntity>().Add(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var existing = await GetByIdAsync(entity.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} with ID {entity.Id} was not found.");

        Context.Entry(existing).CurrentValues.SetValues(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} with ID {id} was not found.");

        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }
}
