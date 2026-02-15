using Microsoft.AspNetCore.Mvc;
using WMovie2Gether.Application.Interfaces;
using WMovie2Gether.Domain.DTOs.User;
using WMovie2Gether.Domain.ValueObjects;

namespace WMovie2Gether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await userService.GetAllAsync(cancellationToken);
    }

    [HttpGet("{id:long}")]
    public async Task<Result<UserDto>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await userService.GetByIdAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<Result<UserDto>> CreateAsync([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        return await userService.CreateAsync(createUserDto, cancellationToken);
    }

    [HttpPut("{id:long}")]
    public async Task<Result<UserDto>> UpdateAsync(long id, [FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        return await userService.UpdateAsync(id, updateUserDto, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        return await userService.DeleteAsync(id, cancellationToken);
    }
}
