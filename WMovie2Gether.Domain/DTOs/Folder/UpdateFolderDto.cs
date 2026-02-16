using System.ComponentModel.DataAnnotations;

namespace WMovie2Gether.Domain.DTOs.Folder;

public class UpdateFolderDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
