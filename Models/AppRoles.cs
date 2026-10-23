namespace HireLink.Models;

/// <summary>
/// Single source of truth for the roles supported by the application.
/// </summary>
public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Recruiter = "Recruiter";
    public const string Student = "Student";

    public static readonly IReadOnlyList<string> All = new[] { Admin, Recruiter, Student };

    public static bool IsValid(string? role) =>
        !string.IsNullOrWhiteSpace(role) && All.Contains(role.Trim(), StringComparer.Ordinal);
}
