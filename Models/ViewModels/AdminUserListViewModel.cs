namespace HireLink.Models.ViewModels;

/// <summary>
/// Server-side user list used by the admin user management screen.
/// </summary>
public class AdminUserListViewModel
{
    public string? Search { get; init; }
    public string? Role { get; init; }
    public bool? IsActive { get; init; }
    public PagedList<AdminUserItem> Users { get; init; } = new();
    public IReadOnlyList<string> AvailableRoles { get; init; } = Array.Empty<string>();

    public PaginationInfo Pagination => new()
    {
        Controller = "Admin",
        Action = "Users",
        Page = Users.Page,
        TotalPages = Users.TotalPages,
        TotalCount = Users.TotalCount,
        PageSize = Users.PageSize,
        RouteValues = new Dictionary<string, string?>
        {
            ["search"] = Search,
            ["role"] = Role,
            ["isActive"] = IsActive?.ToString().ToLowerInvariant()
        }
    };
}

public class AdminUserItem
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public int JobCount { get; init; }
    public bool IsCurrentUser { get; init; }
}
