using System.ComponentModel.DataAnnotations;

namespace Job.Models
{
    public class JobCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<CompanyJobPost> JobPosts { get; set; } = new List<CompanyJobPost>();
    }
}
