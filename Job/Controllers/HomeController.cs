using Job.EF;
using Job.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Job.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Display All Published Job Posts
        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            pageSize = pageSize switch
            {
                6 => 6,
                9 => 9,
                _ => 12
            };

            var query = _context.CompanyJobPosts
                .AsNoTracking()
                .Include(j => j.JobCategory)
                .Where(j => j.PostStatus == "published")
                .OrderByDescending(j => j.JobPostId);

            int totalJobs = query.Count();
            int totalPages = (int)Math.Ceiling(totalJobs / (double)pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var jobs = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalJobs = totalJobs;
            ViewBag.PageSize = pageSize;

            return View(jobs);
        }

        // Display Complete Job Details
        public IActionResult Details(int id)
        {
            var job = _context.CompanyJobPosts
                .AsNoTracking()
                .Include(j => j.JobCategory)
                .Include(j => j.Company)
                .FirstOrDefault(j => j.JobPostId == id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }



        // Apply Now - Check User Role and Redirect Accordingly
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            var job = await _context.CompanyJobPosts.FindAsync(id);

            if (job == null || job.PostStatus != "published")
                return NotFound();

            if (User.IsInRole("company"))
            {
                TempData["Message"] = "Company accounts cannot apply for jobs.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Details", new { id });
            }
            else if (User.IsInRole("admin"))
            {
                TempData["Message"] = "Admin accounts cannot apply for jobs.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Details", new { id });
            }
           

            if (!User.IsInRole("job_seeker"))
                return Forbid();

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);

            if (userId <= 0)
                return Forbid();

            // Check Duplicate Application
            bool alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.UserId == userId && a.JobPostId == id);

            if (alreadyApplied)
            {
                TempData["Message"] = "You have already applied for this job.";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Details", new { id });
            }

            // Save New Application
            var application = new JobApplication
            {
                UserId = userId,
                JobPostId = id,
                ApplicationStatus = "submitted",
                AppliedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Application submitted successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Details", new { id });
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
