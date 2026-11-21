using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobSeekerSavedJob
    {
        [Key]
        public int SavedJobId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int JobPostId { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key Relationships
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(JobPostId))]
        public CompanyJobPost? CompanyJobPost { get; set; }
    }
}
