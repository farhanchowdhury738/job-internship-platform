using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CompanyLogo { get; set; }

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string? LinkedInUrl { get; set; }

        // Foreign Key Relationship
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        // Navigation Property
        public ICollection<CompanyJobPost> JobPosts { get; set; } = new List<CompanyJobPost>();
    }
}
