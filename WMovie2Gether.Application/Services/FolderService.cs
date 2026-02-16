using AutoMapper;
using WMovie2Gether.Application.Interfaces;
using WMovie2Gether.Domain.DTOs.Folder;
using WMovie2Gether.Domain.Entities;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Domain.Resources;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.Application.Services;

public class FolderService : IFolderService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IFolderRepository folderRepository;
    private readonly IUserRepository userRepository;

    public FolderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.folderRepository = unitOfWork.GetRepository<IFolderRepository>();
        this.userRepository = unitOfWork.GetRepository<IUserRepository>();
    }

    public async Task<Result<FolderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        Folder? folder = await folderRepository.GetByIdAsync(id, cancellationToken);

        if (folder == null)
        {
            return Result<FolderDto>.Failure(FolderMessages.FolderNotFound);
        }

        FolderDto folderDto = mapper.Map<FolderDto>(folder);
        return Result<FolderDto>.Success(folderDto, FolderMessages.FolderRetrievedSuccessfully);
    }

    public async Task<Result<IEnumerable<FolderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Folder> folders = await folderRepository.GetAllAsync(cancellationToken);
        IEnumerable<FolderDto> folderDtos = mapper.Map<IEnumerable<FolderDto>>(folders);
        return Result<IEnumerable<FolderDto>>.Success(folderDtos, FolderMessages.FoldersRetrievedSuccessfully);
    }

    public async Task<Result<IEnumerable<FolderDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<IEnumerable<FolderDto>>.Failure(FolderMessages.UserNotFoundForFolder);
        }

        IEnumerable<Folder> folders = await folderRepository.GetByUserIdAsync(userId, cancellationToken);
        IEnumerable<FolderDto> folderDtos = mapper.Map<IEnumerable<FolderDto>>(folders);
        return Result<IEnumerable<FolderDto>>.Success(folderDtos, FolderMessages.FoldersRetrievedSuccessfully);
    }

    public async Task<Result<FolderDto>> CreateAsync(CreateFolderDto createFolderDto, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(createFolderDto.UserId, cancellationToken);
        if (user == null)
        {
            return Result<FolderDto>.Failure(FolderMessages.UserNotFoundForFolder);
        }

        bool isNameUnique = await folderRepository.IsNameUniqueForUserAsync(createFolderDto.Name, createFolderDto.UserId, cancellationToken);
        if (!isNameUnique)
        {
            return Result<FolderDto>.Failure(FolderMessages.FolderNameAlreadyExists);
        }

        Folder folder = Folder.Create(createFolderDto.Name, createFolderDto.UserId, createFolderDto.Description);

        await folderRepository.AddAsync(folder, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        FolderDto folderDto = mapper.Map<FolderDto>(folder);
        return Result<FolderDto>.Success(folderDto, FolderMessages.FolderCreatedSuccessfully);
    }

    public async Task<Result<FolderDto>> UpdateAsync(long id, UpdateFolderDto updateFolderDto, CancellationToken cancellationToken = default)
    {
        Folder? folder = await folderRepository.GetByIdAsync(id, cancellationToken);

        if (folder == null)
        {
            return Result<FolderDto>.Failure(FolderMessages.FolderNotFound);
        }

        if (!string.IsNullOrWhiteSpace(updateFolderDto.Name) && updateFolderDto.Name != folder.Name)
        {
            bool isNameUnique = await folderRepository.IsNameUniqueForUserAsync(updateFolderDto.Name, folder.UserId, cancellationToken);
            if (!isNameUnique)
            {
                return Result<FolderDto>.Failure(FolderMessages.FolderNameAlreadyExists);
            }
        }

        folder.Update(updateFolderDto.Name, updateFolderDto.Description);

        folderRepository.Update(folder);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        FolderDto folderDto = mapper.Map<FolderDto>(folder);
        return Result<FolderDto>.Success(folderDto, FolderMessages.FolderUpdatedSuccessfully);
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        Folder? folder = await folderRepository.GetByIdAsync(id, cancellationToken);

        if (folder == null)
        {
            return Result.Failure(FolderMessages.FolderNotFound);
        }

        folder.Deactivate();
        folderRepository.Update(folder);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(FolderMessages.FolderDeletedSuccessfully);
    }
}
