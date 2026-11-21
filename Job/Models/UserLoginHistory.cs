
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job.Models
{
    [Table("UserLoginHistory")]
    public class UserLoginHistory
    {
        [Key]
        public long LoginId { get; set; }

        public int UserId { get; set; }

        // Login Information
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        // Device Information
        [MaxLength(150)]
        public string? BrowserName { get; set; }

        [MaxLength(150)]
        public string? OperatingSystem { get; set; }

        [MaxLength(150)]
        public string? DeviceName { get; set; }

        [MaxLength(1000)]
        public string? UserAgent { get; set; }

        // Session Information
        [MaxLength(255)]
        public string? SessionId { get; set; }

        public bool IsActive { get; set; } = true;

        // Location Information
        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        // Logout Information
        [MaxLength(100)]
        public string? LogoutReason { get; set; }
    }
}
