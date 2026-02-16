using Microsoft.EntityFrameworkCore;
using WMovie2Gether.Domain.Entities;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Infrastructure.Data;

namespace WMovie2Gether.Infrastructure.Repositories;

public class FolderRepository : BaseRepository<Folder>, IFolderRepository
{
    public FolderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Folder?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
    }

    public override async Task<IEnumerable<Folder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(f => f.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Folder>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(f => f.UserId == userId && f.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Folder?> GetByNameAndUserIdAsync(string name, long userId, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(f => f.Name == name && f.UserId == userId && f.IsActive, cancellationToken);
    }

    public async Task<bool> IsNameUniqueForUserAsync(string name, long userId, CancellationToken cancellationToken = default)
    {
        return !await dbSet.AnyAsync(f => f.Name == name && f.UserId == userId && f.IsActive, cancellationToken);
    }
}
