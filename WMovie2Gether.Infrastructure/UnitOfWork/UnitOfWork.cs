using Microsoft.EntityFrameworkCore.Storage;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Infrastructure.Data;
using WMovie2Gether.Infrastructure.Repositories;

namespace WMovie2Gether.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private IDbContextTransaction? transaction;
    private bool disposed;
    private readonly Dictionary<Type, object> repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
        this.repositories = new Dictionary<Type, object>();
    }

    public TRepository GetRepository<TRepository>() where TRepository : class
    {
        Type repositoryType = typeof(TRepository);

        if (repositories.TryGetValue(repositoryType, out object? existingRepository))
        {
            return (TRepository)existingRepository;
        }

        object repository = repositoryType switch
        {
            _ when repositoryType == typeof(IUserRepository) => new UserRepository(context),
            _ => throw new InvalidOperationException($"Repository of type {repositoryType.Name} is not registered.")
        };

        repositories[repositoryType] = repository;
        return (TRepository)repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.CommitAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                transaction?.Dispose();
                context.Dispose();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
