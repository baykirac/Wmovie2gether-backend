using Microsoft.EntityFrameworkCore;
using WMovie2Gether.Domain.Entities;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Infrastructure.Data;

namespace WMovie2Gether.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(u => u.Id == id && u.IsActive, cancellationToken);
    }

    public override async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(u => u.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(u => u.Email == email && u.IsActive, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(u => u.Username == username && u.IsActive, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await dbSet.AnyAsync(u => u.Username == username, cancellationToken);
    }
}
