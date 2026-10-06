namespace JobInternshipPlatform.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Industry { get; set; } = "";
        public string Location { get; set; } = "";
        public string Size { get; set; } = "";
        public string Website { get; set; } = "";
        public string About { get; set; } = "";
        public int Founded { get; set; }
        public int OpenJobs { get; set; }
        public string Color { get; set; } = "#4f46e5";
        public string Status { get; set; } = "Verified"; // Verified | Pending | Suspended
        public string Initials => string.Concat(Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0])).ToUpper();
    }
}
