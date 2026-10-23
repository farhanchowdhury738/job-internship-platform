using System.ComponentModel.DataAnnotations;

namespace HireLink.Models.ViewModels;

/// <summary>
/// Dedicated input model for activating or deactivating an account.
/// </summary>
public class ChangeUserStatusViewModel
{
    [Required]
    public int UserId { get; set; }

    // Filters that should be preserved after the POST completes.
    public string? ReturnSearch { get; set; }
    public string? ReturnRole { get; set; }
    public string? ReturnIsActive { get; set; }
    public int ReturnPage { get; set; } = 1;
}
