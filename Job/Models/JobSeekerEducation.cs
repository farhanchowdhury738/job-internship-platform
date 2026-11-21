using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobSeekerEducation
    {
        [Key]
        public int EducationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string InstitutionName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Department { get; set; } = string.Empty;

        public decimal Gpa { get; set; }

        // Foreign Key Relationship
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
