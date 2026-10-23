using HireLink.Data;
using HireLink.Models;
using HireLink.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HireLink.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private const int DefaultPageSize = 10;
    private const int DashboardRecentJobs = 5;

    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    // ---------------------------------------------------------------------
    // Dashboard
    // ---------------------------------------------------------------------

    public async Task<IActionResult> Index()
    {
        var totalUsers = await _context.Users.AsNoTracking().CountAsync();
        var totalJobs = await _context.Jobs.AsNoTracking().CountAsync();
        var totalRecruiters = await _context.Users.AsNoTracking()
            .CountAsync(u => u.Role == AppRoles.Recruiter);
        var totalAdmins = await _context.Users.AsNoTracking()
            .CountAsync(u => u.Role == AppRoles.Admin);
        var totalStudents = await _context.Users.AsNoTracking()
            .CountAsync(u => u.Role == AppRoles.Student);
        var activeUsers = await _context.Users.AsNoTracking()
            .CountAsync(u => u.IsActive);
        var inactiveUsers = totalUsers - activeUsers;

        var recentJobs = await _context.Jobs
            .AsNoTracking()
            .OrderByDescending(j => j.PostedAt)
            .ThenByDescending(j => j.Id)
            .Take(DashboardRecentJobs)
            .Select(j => new AdminRecentJobItem
            {
                Id = j.Id,
                Title = j.Title,
                RecruiterName = j.User != null ? j.User.Name : "Unknown recruiter",
                Location = j.Location,
                Salary = j.Salary,
                PostedAt = j.PostedAt
            })
            .ToListAsync();

        var model = new AdminDashboardViewModel
        {
            TotalUsers = totalUsers,
            TotalJobs = totalJobs,
            TotalRecruiters = totalRecruiters,
            TotalAdmins = totalAdmins,
            TotalStudents = totalStudents,
            ActiveUsers = activeUsers,
            InactiveUsers = inactiveUsers,
            RecentJobs = recentJobs
        };

        return View(model);
    }

    // ---------------------------------------------------------------------
    // Job management
    // ---------------------------------------------------------------------

    public async Task<IActionResult> Jobs(string? search, string? location, int page = 1)
    {
        page = page < 1 ? 1 : page;

        var query = _context.Jobs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(j => j.Title.Contains(term)
                || (j.Location != null && j.Location.Contains(term))
                || (j.User != null && j.User.Name.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            var locationFilter = location.Trim();
            query = query.Where(j => j.Location != null && j.Location == locationFilter);
        }

        var totalCount = await query.CountAsync();

        // Clamp when filters change or the last page is emptied by a deletion.
        var totalPages = (int)Math.Ceiling(totalCount / (double)DefaultPageSize);
        if (totalPages > 0 && page > totalPages) page = totalPages;

        var jobs = await query
            .OrderByDescending(j => j.PostedAt)
            .ThenByDescending(j => j.Id)
            .Skip((page - 1) * DefaultPageSize)
            .Take(DefaultPageSize)
            .Select(j => new AdminJobItem
            {
                Id = j.Id,
                Title = j.Title,
                RecruiterName = j.User != null ? j.User.Name : "Unknown recruiter",
                RecruiterEmail = j.User != null ? j.User.Email : string.Empty,
                Location = j.Location,
                Salary = j.Salary,
                PostedAt = j.PostedAt
            })
            .ToListAsync();

        var locations = await _context.Jobs.AsNoTracking()
            .Where(j => j.Location != null && j.Location != string.Empty)
            .Select(j => j.Location!)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();

        return View(new AdminJobListViewModel
        {
            Search = search,
            Location = location,
            Locations = locations,
            Jobs = PagedList<AdminJobItem>.Create(jobs, page, DefaultPageSize, totalCount)
        });
    }

    public async Task<IActionResult> DeleteJob(int id)
    {
        if (id <= 0) return NotFound();

        var job = await _context.Jobs
            .AsNoTracking()
            .Include(j => j.User)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
        {
            TempData["Error"] = "That job could not be found. It may have already been deleted.";
            return RedirectToAction(nameof(Jobs));
        }

        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJobConfirmed(int id, string? returnSearch, string? returnLocation, int returnPage = 1)
    {
        if (id <= 0)
        {
            TempData["Error"] = "Invalid job reference.";
            return RedirectToAction(nameof(Jobs));
        }

        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job == null)
        {
            TempData["Error"] = "That job could not be found. It may have already been deleted.";
            return RedirectToAction(nameof(Jobs), new { search = returnSearch, location = returnLocation, page = returnPage });
        }

        // Only the job posting is removed; the recruiter account and all of
        // their other postings are left untouched.
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Job \u201c{job.Title}\u201d was deleted successfully.";
        return RedirectToAction(nameof(Jobs), new { search = returnSearch, location = returnLocation, page = returnPage });
    }

    // ---------------------------------------------------------------------
    // User management
    // ---------------------------------------------------------------------

    public async Task<IActionResult> Users(string? search, string? role, string? isActive, int page = 1)
    {
        page = page < 1 ? 1 : page;

        bool? statusFilter = bool.TryParse(isActive, out var parsedStatus) ? parsedStatus : null;
        var roleFilter = AppRoles.IsValid(role) ? role!.Trim() : null;

        var query = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(u => u.Name.Contains(term) || u.Email.Contains(term));
        }

        if (roleFilter != null)
            query = query.Where(u => u.Role == roleFilter);

        if (statusFilter.HasValue)
            query = query.Where(u => u.IsActive == statusFilter.Value);

        var totalCount = await query.CountAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)DefaultPageSize);
        if (totalPages > 0 && page > totalPages) page = totalPages;

        var currentUserId = GetCurrentUserId();

        var users = await query
            .OrderBy(u => u.Name)
            .ThenBy(u => u.Id)
            .Skip((page - 1) * DefaultPageSize)
            .Take(DefaultPageSize)
            .Select(u => new AdminUserItem
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                JobCount = _context.Jobs.Count(j => j.UserId == u.Id),
                IsCurrentUser = u.Id == currentUserId
            })
            .ToListAsync();

        return View(new AdminUserListViewModel
        {
            Search = search,
            Role = roleFilter,
            IsActive = statusFilter,
            AvailableRoles = AppRoles.All,
            Users = PagedList<AdminUserItem>.Create(users, page, DefaultPageSize, totalCount)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(ChangeUserRoleViewModel model)
    {
        var redirect = BuildUsersRedirect(model.ReturnSearch, model.ReturnRole, model.ReturnIsActive, model.ReturnPage);

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please choose a valid role.";
            return redirect;
        }

        if (!AppRoles.IsValid(model.Role))
        {
            TempData["Error"] = "That role is not recognised by the application.";
            return redirect;
        }

        var newRole = model.Role.Trim();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
        if (user == null)
        {
            TempData["Error"] = "That user could not be found.";
            return redirect;
        }

        if (user.Role == newRole)
        {
            TempData["Message"] = $"{user.Name} already has the {newRole} role.";
            return redirect;
        }

        if (user.Id == GetCurrentUserId())
        {
            TempData["Error"] = "You cannot change your own role.";
            return redirect;
        }

        // The last remaining admin must never lose admin access.
        if (user.Role == AppRoles.Admin && newRole != AppRoles.Admin)
        {
            var otherActiveAdmins = await _context.Users.CountAsync(u =>
                u.Id != user.Id && u.Role == AppRoles.Admin && u.IsActive);

            if (otherActiveAdmins == 0)
            {
                TempData["Error"] = "The last administrator cannot be demoted. Promote another user to Admin first.";
                return redirect;
            }
        }

        user.Role = newRole;
        await _context.SaveChangesAsync();

        TempData["Message"] = $"{user.Name} was moved to the {newRole} role.";
        return redirect;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(ChangeUserStatusViewModel model)
    {
        var redirect = BuildUsersRedirect(model.ReturnSearch, model.ReturnRole, model.ReturnIsActive, model.ReturnPage);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
        if (user == null)
        {
            TempData["Error"] = "That user could not be found.";
            return redirect;
        }

        if (user.Id == GetCurrentUserId())
        {
            TempData["Error"] = "You cannot deactivate your own account.";
            return redirect;
        }

        if (!user.IsActive)
        {
            TempData["Message"] = $"{user.Name} is already deactivated.";
            return redirect;
        }

        if (user.Role == AppRoles.Admin)
        {
            var otherActiveAdmins = await _context.Users.CountAsync(u =>
                u.Id != user.Id && u.Role == AppRoles.Admin && u.IsActive);

            if (otherActiveAdmins == 0)
            {
                TempData["Error"] = "The last active administrator cannot be deactivated.";
                return redirect;
            }
        }

        // Deactivation only flips the status flag: the account and all of its
        // job postings are preserved.
        user.IsActive = false;
        await _context.SaveChangesAsync();

        TempData["Message"] = $"{user.Name} has been deactivated and can no longer sign in.";
        return redirect;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(ChangeUserStatusViewModel model)
    {
        var redirect = BuildUsersRedirect(model.ReturnSearch, model.ReturnRole, model.ReturnIsActive, model.ReturnPage);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
        if (user == null)
        {
            TempData["Error"] = "That user could not be found.";
            return redirect;
        }

        if (user.IsActive)
        {
            TempData["Message"] = $"{user.Name} is already active.";
            return redirect;
        }

        user.IsActive = true;
        await _context.SaveChangesAsync();

        TempData["Message"] = $"{user.Name} has been reactivated.";
        return redirect;
    }

    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------

    private int GetCurrentUserId() =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    private RedirectToActionResult BuildUsersRedirect(string? search, string? role, string? isActive, int page) =>
        RedirectToAction(nameof(Users), new
        {
            search = string.IsNullOrWhiteSpace(search) ? null : search.Trim(),
            role = AppRoles.IsValid(role) ? role!.Trim() : null,
            isActive = bool.TryParse(isActive, out var s) ? s.ToString().ToLowerInvariant() : null,
            page = page < 1 ? 1 : page
        });
}
