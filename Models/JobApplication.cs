namespace JobInternshipPlatform.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string CompanyColor { get; set; } = "#4f46e5";
        public string ApplicantName { get; set; } = "";
        public string ApplicantTitle { get; set; } = "";
        public string Email { get; set; } = "";
        public string Location { get; set; } = "";
        public int ExperienceYears { get; set; }
        public int MatchScore { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public List<string> Skills { get; set; } = new();
        public string Initials => string.Concat(ApplicantName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0])).ToUpper();
        public string CompanyInitials => string.Concat(CompanyName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0])).ToUpper();
    }
}
