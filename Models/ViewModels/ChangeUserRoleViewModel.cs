using System.ComponentModel.DataAnnotations;

namespace HireLink.Models.ViewModels;

/// <summary>
/// Dedicated input model for changing a user's role. The User entity is never
/// bound directly from the request.
/// </summary>
public class ChangeUserRoleViewModel
{
    [Required]
    public int UserId { get; set; }

    [Required, StringLength(20)]
    public string Role { get; set; } = string.Empty;

    // Filters that should be preserved after the POST completes.
    public string? ReturnSearch { get; set; }
    public string? ReturnRole { get; set; }
    public string? ReturnIsActive { get; set; }
    public int ReturnPage { get; set; } = 1;
}
