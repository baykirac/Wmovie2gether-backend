using WMovie2Gether.Domain.DTOs.User;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserDto>> CreateAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateAsync(long id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
