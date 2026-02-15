namespace WMovie2Gether.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    TRepository GetRepository<TRepository>() where TRepository : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
