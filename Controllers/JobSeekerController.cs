using JobInternshipPlatform.Data;
using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    // TODO (back end): [Authorize(Roles = "JobSeeker")]
    public class JobSeekerController : Controller
    {
        public IActionResult Dashboard()
        {
            var vm = new SeekerDashboardViewModel
            {
                Profile = MockData.Profile,
                RecentApplications = MockData.MyApplications.OrderByDescending(a => a.AppliedDate).Take(4).ToList(),
                Recommended = MockData.Jobs.Where(j => j.Status == "Active" && !j.IsSaved).Take(3).ToList(),
                TotalApplied = MockData.MyApplications.Count,
                Interviews = MockData.MyApplications.Count(a => a.Status == ApplicationStatus.Interview),
                Saved = MockData.Jobs.Count(j => j.IsSaved),
                Unread = MockData.SeekerNotifications.Count(n => !n.IsRead)
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Profile() => View(MockData.Profile);

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Profile(UserProfile model, IFormFile? cv)
        {
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Applications(string? status)
        {
            var all = MockData.MyApplications;
            var vm = new ApplicationsViewModel
            {
                Status = status,
                Counts = Enum.GetNames<ApplicationStatus>().ToDictionary(n => n, n => all.Count(a => a.Status.ToString() == n)),
                Items = all.Where(a => string.IsNullOrEmpty(status) || a.Status.ToString() == status)
                           .OrderByDescending(a => a.AppliedDate).ToList()
            };
            return View(vm);
        }

        public IActionResult SavedJobs() => View(MockData.Jobs.Where(j => j.IsSaved).ToList());

        public IActionResult Notifications() => View(MockData.SeekerNotifications);
    }
}
