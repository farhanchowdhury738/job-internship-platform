using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobPostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ApplicationStatus { get; set; } = "submitted";

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key Relationships
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(JobPostId))]
        public CompanyJobPost? CompanyJobPost { get; set; }
    }
}
