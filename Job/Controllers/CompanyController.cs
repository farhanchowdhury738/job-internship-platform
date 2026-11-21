
using Job.EF;
using Job.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Job.Controllers
{
    [Authorize(Roles = "company")]
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;

        public CompanyController(AppDbContext context)
        {
            _context = context;
        }

        // Company Dashboard
        public IActionResult Dashboard()
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);

            if (user == null)
                return NotFound();

            var company = _context.Companies
                .FirstOrDefault(c => c.UserId == userId);

            // welcome name from company profile if exists, otherwise from user profile
            ViewBag.CompanyName = company?.CompanyName ?? user.FullName;

            ViewBag.JobPostCount = company == null
                ? 0
                : _context.CompanyJobPosts.Count(
                    j => j.CompanyId == company.CompanyId);

            return View(user);
        }


        // GET: Company/InterviewApplicants
        public async Task<IActionResult> InterviewApplicants()
        {
            // Get logged-in user ID
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");

            // Find company
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return RedirectToAction("Login", "Account");

            // Get interview applicants for this company's job posts
            var applicants = await _context.JobApplications
                .AsNoTracking()
                .Include(a => a.User)
                .Include(a => a.CompanyJobPost)
                .Where(a =>
                    a.CompanyJobPost.CompanyId == company.CompanyId &&
                    a.ApplicationStatus == "interview")
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync();

            return View(applicants);
        }


        // GET: Company/RejectedApplicants
        public async Task<IActionResult> RejectedApplicants()
        {
            // Get logged-in user ID
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");

            // Find company
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return RedirectToAction("Login", "Account");

            // Get rejected applicants for this company's job posts
            var applicants = await _context.JobApplications
                .AsNoTracking()
                .Include(a => a.User)
                .Include(a => a.CompanyJobPost)
                .Where(a =>
                    a.CompanyJobPost.CompanyId == company.CompanyId &&
                    a.ApplicationStatus == "rejected")
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync();

            return View(applicants);
        }


        // View Job Applicants - Display
        [HttpGet]
        public async Task<IActionResult> ViewJobApplicants(int id)
        {
            // Get Logged-in Company User ID
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            // Find Logged-in Company
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return NotFound();

            // Verify Job Post Ownership
            var jobPost = await _context.CompanyJobPosts
                .FirstOrDefaultAsync(j =>
                    j.JobPostId == id &&
                    j.CompanyId == company.CompanyId);

            if (jobPost == null)
                return NotFound();

            // Get Applicants with Their Skills
            var applicants = await _context.JobApplications
                .AsNoTracking()
                .Include(a => a.User)
                    .ThenInclude(u => u!.Skills)
                .Where(a => a.JobPostId == id)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();

            // Job Information
            ViewBag.JobPostId = jobPost.JobPostId;
            ViewBag.JobTitle = jobPost.JobTitle;
            ViewBag.JobType = jobPost.JobType;
            ViewBag.Location = jobPost.Location;

            // Applicant Statistics
            ViewBag.TotalApplicants = applicants.Count;
            ViewBag.Submitted = applicants.Count(a => a.ApplicationStatus == "submitted");
            ViewBag.UnderReview = applicants.Count(a => a.ApplicationStatus == "under_review");
            ViewBag.Shortlisted = applicants.Count(a => a.ApplicationStatus == "shortlisted");
            ViewBag.Interview = applicants.Count(a => a.ApplicationStatus == "interview");
            ViewBag.Selected = applicants.Count(a => a.ApplicationStatus == "selected");
            ViewBag.Rejected = applicants.Count(a => a.ApplicationStatus == "rejected");
            ViewBag.OnHold = applicants.Count(a => a.ApplicationStatus == "on_hold");

            return View(applicants);
        }


        // Update Applicant Application Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId, string status)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            // Find Logged-in Company
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return NotFound();

            // Allowed Application Statuses
            var allowedStatuses = new[]
            {
                "submitted",
                "under_review",
                "shortlisted",
                "interview",
                "selected",
                "rejected"

            };

            if (string.IsNullOrWhiteSpace(status) ||
                !allowedStatuses.Contains(status))
                return BadRequest("Invalid application status.");

            // Find Application Belonging to This Company
            var application = await _context.JobApplications
                .Include(a => a.CompanyJobPost)
                .FirstOrDefaultAsync(a =>
                    a.ApplicationId == applicationId &&
                    a.CompanyJobPost!.CompanyId == company.CompanyId);

            if (application == null)
                return NotFound();

            // Update Application Status
            application.ApplicationStatus = status;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Message"] =
                "Application status updated successfully!";

            return RedirectToAction(
                nameof(ViewJobApplicants),
                new { id = application.JobPostId });
        }


        // Company Profile - Display
        [HttpGet]
        public async Task<IActionResult> CompanyProfile()
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                company = new Company();

            // Get Company Email from Users table
            ViewBag.CompanyEmail = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            return View(company);
        }

        // Company Profile - Create or Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyProfile(
            [Bind("CompanyName,CompanyLogo,Description,WebsiteUrl,LinkedInUrl")]
                  Company model)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.CompanyEmail = await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();

                return View(model);
            }

            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingCompany == null)
            {
                model.UserId = userId;
                _context.Companies.Add(model);

                TempData["Message"] =
                    "Company profile created successfully!";
            }
            else
            {
                existingCompany.CompanyName = model.CompanyName;
                existingCompany.CompanyLogo = model.CompanyLogo;
                existingCompany.Description = model.Description;
                existingCompany.WebsiteUrl = model.WebsiteUrl;
                existingCompany.LinkedInUrl = model.LinkedInUrl;

                TempData["Message"] =
                    "Company profile updated successfully!";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CompanyProfile));
        }

        // Manage Job Posts - Display
        [HttpGet]
        public async Task<IActionResult> ManageJobs(int? editId)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                ViewBag.JobPosts = new List<CompanyJobPost>();
                ViewBag.ProfileIncomplete = true;

                await LoadJobCategories();

                return View(new CompanyJobPost());
            }

            ViewBag.ProfileIncomplete = false;

            await LoadJobPosts(company.CompanyId);
            await LoadJobCategories();

            // Load Selected Post for Editing
            if (editId.HasValue)
            {
                var post = await _context.CompanyJobPosts
                    .FirstOrDefaultAsync(j =>
                        j.JobPostId == editId.Value &&
                        j.CompanyId == company.CompanyId);

                if (post == null)
                    return NotFound();

                return View(post);
            }

            return View(new CompanyJobPost());
        }

        // Manage Job Posts - Create or Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageJobs(
            CompanyJobPost model)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                TempData["ProfileWarning"] =
                    "Please complete your company profile before creating a job post.";

                return RedirectToAction(nameof(CompanyProfile));
            }

            // Validate Category
            bool categoryExists = await _context.JobCategories
                .AnyAsync(c => c.CategoryId == model.CategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(
                    nameof(model.CategoryId),
                    "Please select a valid category.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ProfileIncomplete = false;

                await LoadJobPosts(company.CompanyId);
                await LoadJobCategories();

                return View(model);
            }

            // Create New Job Post
            if (model.JobPostId == 0)
            {
                model.CompanyId = company.CompanyId;

                _context.CompanyJobPosts.Add(model);

                TempData["Message"] =
                    "Job post created successfully!";
            }
            else
            {
                var existingPost = await _context.CompanyJobPosts
                    .FirstOrDefaultAsync(j =>
                        j.JobPostId == model.JobPostId &&
                        j.CompanyId == company.CompanyId);

                if (existingPost == null)
                    return NotFound();

                existingPost.CategoryId = model.CategoryId;
                existingPost.JobTitle = model.JobTitle;
                existingPost.JobType = model.JobType;
                existingPost.Vacancy = model.Vacancy;
                existingPost.Location = model.Location;
                existingPost.Salary = model.Salary;
                existingPost.ExperienceRequired = model.ExperienceRequired;
                existingPost.ApplicationDeadline = model.ApplicationDeadline;
                existingPost.Description = model.Description;
                existingPost.PostStatus = model.PostStatus;

                TempData["Message"] =
                    "Job post updated successfully!";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageJobs));
        }

        // Delete Job Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJobPost(int id)
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                TempData["Message"] =
                    "Please complete your company profile first.";

                return RedirectToAction(nameof(ManageJobs));
            }

            var post = await _context.CompanyJobPosts
                .FirstOrDefaultAsync(j =>
                    j.JobPostId == id &&
                    j.CompanyId == company.CompanyId);

            if (post == null)
                return NotFound();

            // Check Existing Applications
            bool hasApplications = await _context.JobApplications
                .AnyAsync(a => a.JobPostId == id);

            if (hasApplications)
            {
                TempData["Message"] =
                    "This job post has applications and cannot be deleted.";

                return RedirectToAction(nameof(ManageJobs));
            }

            _context.CompanyJobPosts.Remove(post);

            await _context.SaveChangesAsync();

            TempData["Message"] =
                "Job post deleted successfully!";

            return RedirectToAction(nameof(ManageJobs));
        }


        // Manage Applicants - Job Post Wise Summary
        [HttpGet]
        public async Task<IActionResult> ManageApplicants()
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
                return RedirectToAction("Login", "Account");

            // Find Logged-in Company
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // If Company Profile Does Not Exist
            if (company == null)
            {
                ViewBag.TotalApplicants = 0;
                ViewBag.TotalPosts = 0;
                ViewBag.JobPosts = new List<CompanyJobPost>();

                return View();
            }

            // Get Company's Job Posts and Applications
            var jobPosts = await _context.CompanyJobPosts
                .AsNoTracking()
                .Include(j => j.Applications)
                .Where(j => j.CompanyId == company.CompanyId)
                .OrderByDescending(j => j.JobPostId)
                .ToListAsync();

            // Total Applicants and Posts
            ViewBag.TotalApplicants = jobPosts.Sum(
                j => j.Applications.Count);

            ViewBag.TotalPosts = jobPosts.Count;

            // Send Job Posts to View
            ViewBag.JobPosts = jobPosts;

            return View();
        }


        // Load Company's Job Posts
        private async Task LoadJobPosts(int companyId)
        {
            ViewBag.JobPosts = await _context.CompanyJobPosts
                .AsNoTracking()
                .Where(j => j.CompanyId == companyId)
                .OrderByDescending(j => j.JobPostId)
                .ToListAsync();
        }

        // Load Job Categories for Dropdown
        private async Task LoadJobCategories()
        {
            ViewBag.JobCategories = new SelectList(
                await _context.JobCategories
                    .AsNoTracking()
                    .ToListAsync(),
                "CategoryId",
                "CategoryName"
            );
        }
    }
}
