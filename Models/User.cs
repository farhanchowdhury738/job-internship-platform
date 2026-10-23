using System.ComponentModel.DataAnnotations;

namespace HireLink.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Role { get; set; } = AppRoles.Student;

    /// <summary>
    /// Database-backed account status. Deactivated users keep their data
    /// (including job postings) but can no longer sign in.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the account was created (displayed as "Joined" in admin views).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
