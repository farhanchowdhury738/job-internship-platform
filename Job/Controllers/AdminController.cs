
using Job.EF;
using Job.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Job.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // Admin Dashboard
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Total Users
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            return View();
        }


        // ==============================
        // User Summary
        // ==============================

        [HttpGet]
        public async Task<IActionResult> UserSummary(string? searchName, string? role, string? activityStatus, int page = 1, int pageSize = 12)
        {
            // Page Size
            pageSize = pageSize switch { 6 => 6, 9 => 9, _ => 12 };
            page = Math.Max(1, page);

            // Main Statistics
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalLogins = await _context.UserLoginHistory.CountAsync();
            ViewBag.TotalLogouts = await _context.UserLoginHistory.CountAsync(x => x.LogoutTime != null);

            // Active Statistics
            ViewBag.ActiveUsers = await _context.UserLoginHistory
                .Where(x => x.IsActive)
                .Select(x => x.UserId)
                .Distinct()
                .CountAsync();

            ViewBag.ActiveSessions = await _context.UserLoginHistory.CountAsync(x => x.IsActive);

            // Role Statistics
            ViewBag.TotalAdmins = await _context.Users.CountAsync(x => x.Role == "admin");
            ViewBag.TotalCompanies = await _context.Users.CountAsync(x => x.Role == "company");
            ViewBag.TotalJobSeekers = await _context.Users.CountAsync(x => x.Role == "job_seeker");

            // Search and Filters
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(u => u.FullName.Contains(searchName.Trim()));

            if (!string.IsNullOrWhiteSpace(role))
                query = query.Where(u => u.Role == role);

            // Activity Status Filter
            if (activityStatus == "active")
                query = query.Where(u => _context.UserLoginHistory.Any(h => h.UserId == u.UserId && h.IsActive));

            else if (activityStatus == "inactive")
                query = query.Where(u => !_context.UserLoginHistory.Any(h => h.UserId == u.UserId && h.IsActive));

            // Pagination
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            // User Login Summary
            var users = await query
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Email,
                    u.Role,
                    TotalLogins = _context.UserLoginHistory.Count(h => h.UserId == u.UserId),
                    TotalLogouts = _context.UserLoginHistory.Count(h => h.UserId == u.UserId && h.LogoutTime != null),
                    ActiveSessions = _context.UserLoginHistory.Count(h => h.UserId == u.UserId && h.IsActive)
                })
                .ToListAsync();

            // View Data
            ViewBag.SearchName = searchName;
            ViewBag.SelectedRole = role;
            ViewBag.ActivityStatus = activityStatus;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = totalPages;

            return View(users);
        }


        // ==============================
        // User Login History
        // ==============================
        [HttpGet]
        public async Task<IActionResult> UserLoginHistory(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserName = user.FullName;
            ViewBag.UserEmail = user.Email;

            var history = await _context.UserLoginHistory
                .Where(x => x.UserId == id)
                .OrderByDescending(x => x.LoginTime)
                .ToListAsync();

            return View(history);
        }
    }
}
