namespace HireLink.Models.ViewModels;

/// <summary>
/// Server-side job list used by the admin job management screen.
/// </summary>
public class AdminJobListViewModel
{
    public string? Search { get; init; }
    public string? Location { get; init; }
    public PagedList<AdminJobItem> Jobs { get; init; } = new();
    public IReadOnlyList<string> Locations { get; init; } = Array.Empty<string>();

    public PaginationInfo Pagination => new()
    {
        Controller = "Admin",
        Action = "Jobs",
        Page = Jobs.Page,
        TotalPages = Jobs.TotalPages,
        TotalCount = Jobs.TotalCount,
        PageSize = Jobs.PageSize,
        RouteValues = new Dictionary<string, string?>
        {
            ["search"] = Search,
            ["location"] = Location
        }
    };
}

public class AdminJobItem
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string RecruiterName { get; init; } = string.Empty;
    public string RecruiterEmail { get; init; } = string.Empty;
    public string? Location { get; init; }
    public decimal? Salary { get; init; }
    public DateTime PostedAt { get; init; }
}
