using JobInternshipPlatform.Data;
using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    // TODO (back end): [Authorize(Roles = "Recruiter")]
    public class RecruiterController : Controller
    {
        private static List<Job> MyJobs => MockData.Jobs.Where(j => j.CompanyId == MockData.RecruiterCompanyId).ToList();

        public IActionResult Dashboard()
        {
            var vm = new RecruiterDashboardViewModel
            {
                Company = MockData.Companies.First(c => c.Id == MockData.RecruiterCompanyId),
                Jobs = MyJobs.Take(4).ToList(),
                RecentApplicants = MockData.Applicants.OrderByDescending(a => a.AppliedDate).Take(5).ToList(),
                ActiveJobs = MyJobs.Count(j => j.Status == "Active"),
                TotalApplicants = MyJobs.Sum(j => j.ApplicantCount),
                Shortlisted = MockData.Applicants.Count(a => a.Status == ApplicationStatus.Shortlisted),
                Views = MyJobs.Sum(j => j.Views),
                WeeklyApplicants = new[] { 12, 19, 8, 24, 31, 18, 27 }
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult CompanyProfile() => View(MockData.Companies.First(c => c.Id == MockData.RecruiterCompanyId));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CompanyProfile(Company model)
        {
            TempData["Success"] = "Company profile saved.";
            return RedirectToAction(nameof(CompanyProfile));
        }

        public IActionResult Jobs(string? status)
        {
            ViewBag.Status = status;
            var list = MyJobs.Where(j => string.IsNullOrEmpty(status) || j.Status == status).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult PostJob() => View(new Job { Type = "Full-time", Level = "Entry", Deadline = DateTime.Today.AddDays(30) });

        [HttpGet]
        public IActionResult EditJob(int id)
        {
            var job = MockData.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null) return NotFound();
            return View("PostJob", job);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SaveJob(Job model)
        {
            TempData["Success"] = model.Id == 0 ? "Job posted successfully!" : "Job updated successfully.";
            return RedirectToAction(nameof(Jobs));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteJob(int id)
        {
            TempData["Success"] = "Job deleted (demo).";
            return RedirectToAction(nameof(Jobs));
        }

        public IActionResult Applicants(int? jobId, string? status)
        {
            var vm = new ApplicantsViewModel
            {
                JobId = jobId,
                Status = status,
                Jobs = MyJobs,
                Items = MockData.Applicants
                    .Where(a => (jobId == null || a.JobId == jobId) && (string.IsNullOrEmpty(status) || a.Status.ToString() == status))
                    .OrderByDescending(a => a.MatchScore).ToList()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, ApplicationStatus status)
        {
            TempData["Success"] = $"Application marked as {status}.";
            return RedirectToAction(nameof(Applicants));
        }

        public IActionResult Notifications() => View(MockData.RecruiterNotifications);
    }
}
