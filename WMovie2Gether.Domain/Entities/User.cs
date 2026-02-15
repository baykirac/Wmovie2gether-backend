using System.ComponentModel.DataAnnotations;

namespace WMovie2Gether.Domain.Entities;

public class User : BaseEntity
{
    private User() { } // EF Core için private constructor

    [Required]
    [MaxLength(50)]
    public string Username { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; private set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; private set; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime? LastLoginAt { get; private set; }

    public static User Create(string username, string email, string passwordHash, string? displayName = null)
    {
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            DisplayName = displayName,
            IsActive = true
        };
    }

    public void Update(string? username, string? displayName, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            Username = username;
        }

        if (displayName != null)
        {
            DisplayName = displayName;
        }

        if (isActive.HasValue)
        {
            IsActive = isActive.Value;
        }
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
