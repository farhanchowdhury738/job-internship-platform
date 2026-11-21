using System.ComponentModel.DataAnnotations;

namespace Job.Models
{
    public class Register
    {
        [Required]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
