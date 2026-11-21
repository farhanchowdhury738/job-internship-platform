using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class CompanyJobPost
    {
        [Key]
        public int JobPostId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string JobType { get; set; } = string.Empty;

        [Required]
        public int Vacancy { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Salary { get; set; }

        [MaxLength(100)]
        public string? ExperienceRequired { get; set; }

        [Required]
        public DateOnly ApplicationDeadline { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PostStatus { get; set; } = "draft";

        // Foreign Key Relationships
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public JobCategory? JobCategory { get; set; }

        // Navigation Properties
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        public ICollection<JobSeekerSavedJob> SavedJobs { get; set; } = new List<JobSeekerSavedJob>();
    }
}
