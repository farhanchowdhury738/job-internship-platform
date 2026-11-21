using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobSeekerProject
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? LiveUrl { get; set; }

        // Foreign Key Relationship
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
