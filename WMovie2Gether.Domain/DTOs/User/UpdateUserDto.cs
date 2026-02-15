using System.ComponentModel.DataAnnotations;

namespace WMovie2Gether.Domain.DTOs.User;

public class UpdateUserDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    public bool? IsActive { get; set; }
}
