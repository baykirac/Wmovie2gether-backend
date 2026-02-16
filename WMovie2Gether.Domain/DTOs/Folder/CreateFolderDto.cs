using System.ComponentModel.DataAnnotations;

namespace WMovie2Gether.Domain.DTOs.Folder;

public class CreateFolderDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public long UserId { get; set; }
}
