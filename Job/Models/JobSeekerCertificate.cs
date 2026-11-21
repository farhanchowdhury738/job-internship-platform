using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    public class JobSeekerCertificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CertificateName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string CertificateUrl { get; set; } = string.Empty;

        // Foreign Key Relationship
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
