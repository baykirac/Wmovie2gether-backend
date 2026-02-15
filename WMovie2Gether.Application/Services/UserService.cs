using AutoMapper;
using WMovie2Gether.Application.Extensions;
using WMovie2Gether.Application.Interfaces;
using WMovie2Gether.Domain.DTOs.User;
using WMovie2Gether.Domain.Entities;
using WMovie2Gether.Domain.Interfaces;
using WMovie2Gether.Domain.Resources;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.userRepository = unitOfWork.GetRepository<IUserRepository>();
    }

    public async Task<Result<UserDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return Result<UserDto>.Failure(UserMessages.UserNotFound);
        }

        UserDto userDto = mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto, UserMessages.UserRetrievedSuccessfully);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<User> users = await userRepository.GetAllAsync(cancellationToken);
        IEnumerable<UserDto> userDtos = mapper.Map<IEnumerable<UserDto>>(users);
        return Result<IEnumerable<UserDto>>.Success(userDtos, UserMessages.UsersRetrievedSuccessfully);
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        bool isEmailUnique = await userRepository.IsEmailUniqueAsync(createUserDto.Email, cancellationToken);
        if (!isEmailUnique)
        {
            return Result<UserDto>.Failure(UserMessages.EmailAlreadyExists);
        }

        bool isUsernameUnique = await userRepository.IsUsernameUniqueAsync(createUserDto.Username, cancellationToken);
        if (!isUsernameUnique)
        {
            return Result<UserDto>.Failure(UserMessages.UsernameAlreadyExists);
        }

        string hashedPassword = createUserDto.Password.HashPassword();
        User user = User.Create(createUserDto.Username, createUserDto.Email, hashedPassword, createUserDto.DisplayName);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        UserDto userDto = mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto, UserMessages.UserCreatedSuccessfully);
    }

    public async Task<Result<UserDto>> UpdateAsync(long id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return Result<UserDto>.Failure(UserMessages.UserNotFound);
        }

        if (!string.IsNullOrWhiteSpace(updateUserDto.Username) && updateUserDto.Username != user.Username)
        {
            bool isUsernameUnique = await userRepository.IsUsernameUniqueAsync(updateUserDto.Username, cancellationToken);
            if (!isUsernameUnique)
            {
                return Result<UserDto>.Failure(UserMessages.UsernameAlreadyExists);
            }
        }

        user.Update(updateUserDto.Username, updateUserDto.DisplayName, updateUserDto.IsActive);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        UserDto userDto = mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto, UserMessages.UserUpdatedSuccessfully);
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserMessages.UserNotFound);
        }

        user.Deactivate();
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(UserMessages.UserDeletedSuccessfully);
    }
}
