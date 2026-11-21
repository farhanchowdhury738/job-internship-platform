
using Job.EF;
using Job.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Job.Controllers
{
    [Authorize(Roles = "job_seeker")]
    public class JobSeekerController : Controller
    {
        private readonly AppDbContext _context;

        public JobSeekerController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // Job Seeker Dashboard
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
                return NotFound();

            // total skill categories
            ViewBag.TotalCategories = await _context.JobSeekerSkills
                .Where(s => s.UserId == userId).Select(s => s.CategoryName).Distinct().CountAsync();

            // total applied jobs
            ViewBag.AppliedJobsCount = await _context.JobApplications
                .CountAsync(a => a.UserId == userId);

            // total projects
            ViewBag.ProjectsCount = await _context.JobSeekerProjects
                .CountAsync(p => p.UserId == userId);

            return View(user);
        }

        // ==============================
        // My Profile - Display
        // ==============================
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId.Value);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
    string fullName,
    string? newPassword)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId.Value);

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError("FullName", "Name is required.");
                return View("MyProfile", user);
            }

            user.FullName = fullName.Trim();

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                user.PasswordHash = newPassword;
            }


            await _context.SaveChangesAsync();

            TempData["Message"] = "Profile updated successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(MyProfile));
        }

        // ==============================
        // My Skills - Display
        // ==============================

        public async Task<IActionResult> MySkills()
        {
            int? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var skills = await _context.JobSeekerSkills
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.CategoryName)
                .ThenBy(s => s.SkillName)
                .ToListAsync();

            return View(skills);
        }


        // Add Skills
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkills(string categoryName, string skillNames)
        {
            int? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            categoryName = categoryName?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(categoryName) ||
                string.IsNullOrWhiteSpace(skillNames))
            {
                TempData["Error"] = "Category and skills are required.";
                return RedirectToAction(nameof(MySkills));
            }

            if (categoryName.Length > 100)
            {
                TempData["Error"] = "Category name cannot exceed 100 characters.";
                return RedirectToAction(nameof(MySkills));
            }

            var names = skillNames
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (names.Any(s => s.Length > 100))
            {
                TempData["Error"] = "Each skill name must be within 100 characters.";
                return RedirectToAction(nameof(MySkills));
            }

            if (!names.Any())
            {
                TempData["Error"] = "Please enter at least one skill.";
                return RedirectToAction(nameof(MySkills));
            }

            var existingSkills = await _context.JobSeekerSkills
                .Where(s => s.UserId == userId &&
                            s.CategoryName == categoryName)
                .Select(s => s.SkillName)
                .ToListAsync();

            var newSkills = names
                .Where(name => !existingSkills.Any(
                    existing => string.Equals(
                        existing, name, StringComparison.OrdinalIgnoreCase)))
                .Select(name => new JobSeekerSkill
                {
                    UserId = userId.Value,
                    CategoryName = categoryName,
                    SkillName = name
                })
                .ToList();

            if (!newSkills.Any())
            {
                TempData["Error"] = "These skills already exist in this category.";
                return RedirectToAction(nameof(MySkills));
            }

            _context.JobSeekerSkills.AddRange(newSkills);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Skills added successfully!";

            return RedirectToAction(nameof(MySkills));
        }


        // Edit Skills
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkills(
            string oldCategoryName,
            string categoryName,
            string skillNames)
        {
            int? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            oldCategoryName = oldCategoryName?.Trim() ?? "";
            categoryName = categoryName?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(oldCategoryName) ||
                string.IsNullOrWhiteSpace(categoryName) ||
                string.IsNullOrWhiteSpace(skillNames))
            {
                TempData["Error"] = "All fields are required.";
                return RedirectToAction(nameof(MySkills));
            }

            if (categoryName.Length > 100)
            {
                TempData["Error"] = "Category name cannot exceed 100 characters.";
                return RedirectToAction(nameof(MySkills));
            }

            var names = skillNames
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!names.Any() || names.Any(s => s.Length > 100))
            {
                TempData["Error"] = "Please enter valid skill names (maximum 100 characters each).";
                return RedirectToAction(nameof(MySkills));
            }

            var oldSkills = await _context.JobSeekerSkills
                .Where(s => s.UserId == userId &&
                            s.CategoryName == oldCategoryName)
                .ToListAsync();

            if (!oldSkills.Any())
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(MySkills));
            }

            _context.JobSeekerSkills.RemoveRange(oldSkills);

            var updatedSkills = names.Select(name => new JobSeekerSkill
            {
                UserId = userId.Value,
                CategoryName = categoryName,
                SkillName = name
            }).ToList();

            _context.JobSeekerSkills.AddRange(updatedSkills);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Skills updated successfully!";

            return RedirectToAction(nameof(MySkills));
        }


        // Delete Skills Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkills(string categoryName)
        {
            int? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var skills = await _context.JobSeekerSkills
                .Where(s => s.UserId == userId &&
                            s.CategoryName == categoryName)
                .ToListAsync();

            if (!skills.Any())
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(MySkills));
            }

            _context.JobSeekerSkills.RemoveRange(skills);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Skill category deleted successfully!";

            return RedirectToAction(nameof(MySkills));
        }


        // ==============================
        // Display Applied Jobs
        // ==============================
        [HttpGet]
        public async Task<IActionResult> AppliedJobs()
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var applications = await _context.JobApplications
                .AsNoTracking()
                .Include(a => a.CompanyJobPost)
                    .ThenInclude(j => j!.Company)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();

            return View(applications);
        }


        // ==============================
        // My Projects - Display
        // ==============================
        [HttpGet]
        public async Task<IActionResult> MyProjects(int? editId)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Load Logged-in User's Projects
            ViewBag.JobProjects = await _context.JobSeekerProjects
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.ProjectId)
                .ToListAsync();

            // Load Selected Project for Editing
            if (editId.HasValue)
            {
                var project = await _context.JobSeekerProjects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.ProjectId == editId.Value &&
                        p.UserId == userId);

                if (project == null)
                    return NotFound();

                return View(project);
            }

            // Empty Model for Create Mode
            return View(new JobSeekerProject());
        }


        // ==============================
        // Create Project
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(
            JobSeekerProject model)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                await LoadMyProjects(userId.Value);
                return View("MyProjects", model);
            }

            model.UserId = userId.Value;

            _context.JobSeekerProjects.Add(model);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Project added successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(MyProjects));
        }


        // ==============================
        // Edit Project
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(
            int id,
            JobSeekerProject model)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (id != model.ProjectId)
                return BadRequest();

            // Verify Project Ownership
            var project = await _context.JobSeekerProjects
                .FirstOrDefaultAsync(p =>
                    p.ProjectId == id &&
                    p.UserId == userId);

            if (project == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadMyProjects(userId.Value);
                return View("MyProjects", model);
            }

            // Update Project Information
            project.ProjectTitle = model.ProjectTitle;
            project.Description = model.Description;
            project.LiveUrl = model.LiveUrl;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Project updated successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(MyProjects));
        }


        // ==============================
        // Delete Project
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Verify Project Ownership
            var project = await _context.JobSeekerProjects
                .FirstOrDefaultAsync(p =>
                    p.ProjectId == id &&
                    p.UserId == userId);

            if (project == null)
                return NotFound();

            _context.JobSeekerProjects.Remove(project);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Project deleted successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(MyProjects));
        }


        // ==============================
        // Load My Projects
        // ==============================
        private async Task LoadMyProjects(int userId)
        {
            ViewBag.JobProjects = await _context.JobSeekerProjects
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.ProjectId)
                .ToListAsync();
        }


        // ==============================
        // Get Logged-in User ID
        // ==============================
        private int? GetUserId()
        {
            var userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdValue, out int userId))
                return userId;

            return null;
        }
    }
}
