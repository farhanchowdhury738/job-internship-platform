namespace JobInternshipPlatform.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = "";
        public string CompanyColor { get; set; } = "#4f46e5";
        public string Location { get; set; } = "";
        public string Type { get; set; } = "Full-time"; // Full-time | Part-time | Internship | Contract
        public string Category { get; set; } = "";
        public string Level { get; set; } = "Entry"; // Entry | Mid | Senior
        public bool IsRemote { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; } = "";
        public List<string> Responsibilities { get; set; } = new();
        public List<string> Requirements { get; set; } = new();
        public List<string> Skills { get; set; } = new();
        public List<string> Benefits { get; set; } = new();
        public int ApplicantCount { get; set; }
        public int Views { get; set; }
        public string Status { get; set; } = "Active"; // Active | Closed | Draft | Pending | Flagged
        public bool IsSaved { get; set; }

        public string CompanyInitials => string.Concat(CompanyName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0])).ToUpper();
        public string SalaryText => Type == "Internship"
            ? $"${SalaryMin:N0} - ${SalaryMax:N0} / month"
            : $"${SalaryMin / 1000}k - ${SalaryMax / 1000}k / year";
        public string PostedAgo
        {
            get
            {
                var d = (DateTime.Today - PostedDate.Date).Days;
                return d <= 0 ? "Today" : d == 1 ? "Yesterday" : d < 14 ? $"{d} days ago" : $"{d / 7} weeks ago";
            }
        }
    }
}
