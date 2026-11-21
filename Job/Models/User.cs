using System.ComponentModel.DataAnnotations;

namespace Job.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;


        // Navigation Properties
        public Company? Company { get; set; }
        public ICollection<JobSeekerSkill> Skills { get; set; } = new List<JobSeekerSkill>();
        public ICollection<JobSeekerProject> Projects { get; set; } = new List<JobSeekerProject>();
        public ICollection<JobSeekerEducation> Educations { get; set; } = new List<JobSeekerEducation>();
        public ICollection<JobSeekerCertificate> Certificates { get; set; } = new List<JobSeekerCertificate>();
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        public ICollection<JobSeekerSavedJob> SavedJobs { get; set; } = new List<JobSeekerSavedJob>();

    }
}
