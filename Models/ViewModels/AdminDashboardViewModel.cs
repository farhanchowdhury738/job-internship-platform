namespace HireLink.Models.ViewModels;

/// <summary>
/// Server-side projection for the admin dashboard.
/// </summary>
public class AdminDashboardViewModel
{
    public int TotalUsers { get; init; }
    public int TotalJobs { get; init; }
    public int TotalRecruiters { get; init; }
    public int ActiveUsers { get; init; }
    public int InactiveUsers { get; init; }
    public int TotalAdmins { get; init; }
    public int TotalStudents { get; init; }

    public IReadOnlyList<AdminRecentJobItem> RecentJobs { get; init; } = Array.Empty<AdminRecentJobItem>();
}

public class AdminRecentJobItem
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string RecruiterName { get; init; } = string.Empty;
    public string? Location { get; init; }
    public decimal? Salary { get; init; }
    public DateTime PostedAt { get; init; }
}
