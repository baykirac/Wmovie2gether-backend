using WMovie2Gether.Domain.DTOs.Folder;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.Application.Interfaces;

public interface IFolderService
{
    Task<Result<FolderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<FolderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<FolderDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<Result<FolderDto>> CreateAsync(CreateFolderDto createFolderDto, CancellationToken cancellationToken = default);
    Task<Result<FolderDto>> UpdateAsync(long id, UpdateFolderDto updateFolderDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
