using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobSeekerSkill
    {
        [Key]
        public int SkillId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        // Foreign Key Relationship
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
