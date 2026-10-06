namespace JobInternshipPlatform.Models
{
    public class HomeViewModel
    {
        public List<Job> FeaturedJobs { get; set; } = new();
        public List<Company> TopCompanies { get; set; } = new();
        public List<(string Name, string Icon, int Count)> Categories { get; set; } = new();
    }

    public class JobSearchViewModel
    {
        public string? Q { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
        public string? Level { get; set; }
        public bool RemoteOnly { get; set; }
        public string Sort { get; set; } = "newest";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);
        public List<Job> Jobs { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }

    public class JobDetailsViewModel
    {
        public Job Job { get; set; } = new();
        public Company Company { get; set; } = new();
        public List<Job> Similar { get; set; } = new();
    }

    public class LoginViewModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
        public string Role { get; set; } = "JobSeeker";
    }

    public class RegisterViewModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string Role { get; set; } = "JobSeeker";
        public string? CompanyName { get; set; }
    }

    public class SeekerDashboardViewModel
    {
        public UserProfile Profile { get; set; } = new();
        public List<JobApplication> RecentApplications { get; set; } = new();
        public List<Job> Recommended { get; set; } = new();
        public int TotalApplied { get; set; }
        public int Interviews { get; set; }
        public int Saved { get; set; }
        public int Unread { get; set; }
    }

    public class ApplicationsViewModel
    {
        public string? Status { get; set; }
        public List<JobApplication> Items { get; set; } = new();
        public Dictionary<string, int> Counts { get; set; } = new();
    }

    public class RecruiterDashboardViewModel
    {
        public Company Company { get; set; } = new();
        public List<Job> Jobs { get; set; } = new();
        public List<JobApplication> RecentApplicants { get; set; } = new();
        public int ActiveJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int Shortlisted { get; set; }
        public int Views { get; set; }
        public int[] WeeklyApplicants { get; set; } = Array.Empty<int>();
    }

    public class ApplicantsViewModel
    {
        public int? JobId { get; set; }
        public string? Status { get; set; }
        public List<Job> Jobs { get; set; } = new();
        public List<JobApplication> Items { get; set; } = new();
    }

    public class AdminDashboardViewModel
    {
        public int Users { get; set; }
        public int Companies { get; set; }
        public int Jobs { get; set; }
        public int OpenReports { get; set; }
        public int Applications { get; set; }
        public int[] MonthlySignups { get; set; } = Array.Empty<int>();
        public List<ActivityItem> Activity { get; set; } = new();
        public List<ReportItem> Reports { get; set; } = new();
        public List<AppUser> NewUsers { get; set; } = new();
    }
}
