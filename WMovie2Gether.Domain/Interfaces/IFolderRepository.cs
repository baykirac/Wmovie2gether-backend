using WMovie2Gether.Domain.Entities;

namespace WMovie2Gether.Domain.Interfaces;

public interface IFolderRepository : IBaseRepository<Folder>
{
    Task<IEnumerable<Folder>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<Folder?> GetByNameAndUserIdAsync(string name, long userId, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueForUserAsync(string name, long userId, CancellationToken cancellationToken = default);
}
