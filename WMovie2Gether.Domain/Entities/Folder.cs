using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMovie2Gether.Domain.Entities;

public class Folder : BaseEntity
{
    private Folder() { } // EF Core için private constructor

    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; private set; }

    public bool IsActive { get; private set; } = true;

    [Required]
    public long UserId { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; private set; } = null!;

    public static Folder Create(string name, long userId, string? description = null)
    {
        return new Folder
        {
            Name = name,
            Description = description,
            UserId = userId,
            IsActive = true
        };
    }

    public void Update(string? name, string? description)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
        }

        if (description != null)
        {
            Description = description;
        }
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}
