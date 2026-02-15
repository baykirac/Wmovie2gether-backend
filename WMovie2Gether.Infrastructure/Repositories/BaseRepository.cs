using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WMovie2Gether.Domain.Entities;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Infrastructure.Data;

namespace WMovie2Gether.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<TEntity> dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual void Update(TEntity entity)
    {
        dbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.AnyAsync(predicate, cancellationToken);
    }
}
