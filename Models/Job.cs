using System.ComponentModel.DataAnnotations;

namespace HireLink.Models;

public class Job
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(3000)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 100000000)]
    public decimal? Salary { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? User { get; set; }
}
