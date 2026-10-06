namespace JobInternshipPlatform.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "JobSeeker"; // JobSeeker | Recruiter | Admin
        public string Status { get; set; } = "Active";  // Active | Suspended | Pending
        public DateTime Joined { get; set; }
        public string Initials => string.Concat(Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0])).ToUpper();
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Icon { get; set; } = "bi-bell";
        public string Tone { get; set; } = "primary"; // primary | success | warning | danger | info
        public string TimeAgo { get; set; } = "";
        public bool IsRead { get; set; }
    }

    public class ReportItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";   // Job | Company | User
        public string Target { get; set; } = "";
        public string ReportedBy { get; set; } = "";
        public string Reason { get; set; } = "";
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Open"; // Open | Reviewing | Resolved | Dismissed
    }

    public class ActivityItem
    {
        public string Text { get; set; } = "";
        public string Actor { get; set; } = "";
        public string Time { get; set; } = "";
        public string Icon { get; set; } = "bi-activity";
        public string Tone { get; set; } = "primary";
    }

    public class UserProfile
    {
        public string FullName { get; set; } = "";
        public string Headline { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Location { get; set; } = "";
        public string About { get; set; } = "";
        public string Education { get; set; } = "";
        public string Experience { get; set; } = "";
        public string Website { get; set; } = "";
        public string CvFileName { get; set; } = "";
        public string CvSize { get; set; } = "";
        public DateTime CvUpdated { get; set; }
        public List<string> Skills { get; set; } = new();
        public int Completion { get; set; }
    }
}
