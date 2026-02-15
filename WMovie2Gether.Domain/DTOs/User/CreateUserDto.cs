using System.ComponentModel.DataAnnotations;

namespace WMovie2Gether.Domain.DTOs.User;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; set; }
}
