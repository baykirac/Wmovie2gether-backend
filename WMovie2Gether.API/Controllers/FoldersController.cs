using Microsoft.AspNetCore.Mvc;
using WMovie2Gether.Application.Interfaces;
using WMovie2Gether.Domain.DTOs.Folder;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IFolderService folderService;

    public FoldersController(IFolderService folderService)
    {
        this.folderService = folderService;
    }

    [HttpGet]
    public async Task<Result<IEnumerable<FolderDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await folderService.GetAllAsync(cancellationToken);
    }

    [HttpGet("{id:long}")]
    public async Task<Result<FolderDto>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await folderService.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet("user/{userId:long}")]
    public async Task<Result<IEnumerable<FolderDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await folderService.GetByUserIdAsync(userId, cancellationToken);
    }

    [HttpPost]
    public async Task<Result<FolderDto>> CreateAsync([FromBody] CreateFolderDto createFolderDto, CancellationToken cancellationToken)
    {
        return await folderService.CreateAsync(createFolderDto, cancellationToken);
    }

    [HttpPut("{id:long}")]
    public async Task<Result<FolderDto>> UpdateAsync(long id, [FromBody] UpdateFolderDto updateFolderDto, CancellationToken cancellationToken)
    {
        return await folderService.UpdateAsync(id, updateFolderDto, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        return await folderService.DeleteAsync(id, cancellationToken);
    }
}
