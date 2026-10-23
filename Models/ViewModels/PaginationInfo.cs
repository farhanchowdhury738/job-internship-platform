namespace HireLink.Models.ViewModels;

/// <summary>
/// Data required to render the admin pagination component.
/// </summary>
public class PaginationInfo
{
    public string Controller { get; init; } = "Admin";
    public string Action { get; init; } = "Index";
    public int Page { get; init; } = 1;
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Query string values (search, filters) that must survive page navigation.
    /// </summary>
    public IDictionary<string, string?> RouteValues { get; init; } = new Dictionary<string, string?>();
}
