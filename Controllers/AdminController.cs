using JobInternshipPlatform.Data;
using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    // TODO (back end): [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            var vm = new AdminDashboardViewModel
            {
                Users = MockData.Users.Count + 1240,
                Companies = MockData.Companies.Count + 86,
                Jobs = MockData.Jobs.Count + 312,
                Applications = 8420,
                OpenReports = MockData.Reports.Count(r => r.Status == "Open" || r.Status == "Reviewing"),
                MonthlySignups = new[] { 80, 112, 95, 140, 168, 190, 176, 220, 248, 260, 301, 342 },
                Activity = MockData.Activity.Take(6).ToList(),
                Reports = MockData.Reports.Where(r => r.Status == "Open" || r.Status == "Reviewing").ToList(),
                NewUsers = MockData.Users.OrderByDescending(u => u.Joined).Take(5).ToList()
            };
            return View(vm);
        }

        public IActionResult Users(string? role, string? q)
        {
            ViewBag.Role = role; ViewBag.Q = q;
            var list = MockData.Users.Where(u => (string.IsNullOrEmpty(role) || u.Role == role)
                && (string.IsNullOrEmpty(q) || u.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || u.Email.Contains(q, StringComparison.OrdinalIgnoreCase)));
            return View(list.ToList());
        }

        public IActionResult Companies(string? status)
        {
            ViewBag.Status = status;
            return View(MockData.Companies.Where(c => string.IsNullOrEmpty(status) || c.Status == status).ToList());
        }

        public IActionResult Jobs(string? status)
        {
            ViewBag.Status = status;
            return View(MockData.Jobs.Where(j => string.IsNullOrEmpty(status) || j.Status == status).ToList());
        }

        public IActionResult Reports(string? status)
        {
            ViewBag.Status = status;
            return View(MockData.Reports.Where(r => string.IsNullOrEmpty(status) || r.Status == status).ToList());
        }

        public IActionResult Activity() => View(MockData.Activity);

        // Demo action endpoints (UI feedback only)
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Moderate(string target, int id, string act, string returnAction)
        {
            TempData["Success"] = $"{target} #{id}: {act} (demo)";
            return RedirectToAction(returnAction);
        }
    }
}
