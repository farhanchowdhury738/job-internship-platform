using System.ComponentModel.DataAnnotations;

namespace HireLink.Models;

public class RegisterViewModel
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    // TEMPORARY (local development only): minimum length relaxed so short
    // plain-text test passwords such as "123" can be used.
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Student";
}
