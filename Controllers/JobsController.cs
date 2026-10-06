using JobInternshipPlatform.Data;
using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index(JobSearchViewModel f)
        {
            var q = MockData.Jobs.Where(j => j.Status == "Active" || j.Status == "Flagged").AsEnumerable();

            if (!string.IsNullOrWhiteSpace(f.Q))
                q = q.Where(j => j.Title.Contains(f.Q, StringComparison.OrdinalIgnoreCase)
                              || j.CompanyName.Contains(f.Q, StringComparison.OrdinalIgnoreCase)
                              || j.Skills.Any(s => s.Contains(f.Q, StringComparison.OrdinalIgnoreCase)));
            if (!string.IsNullOrWhiteSpace(f.Location))
                q = q.Where(j => j.Location.Contains(f.Location, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(f.Type)) q = q.Where(j => j.Type == f.Type);
            if (!string.IsNullOrWhiteSpace(f.Category)) q = q.Where(j => j.Category == f.Category);
            if (!string.IsNullOrWhiteSpace(f.Level)) q = q.Where(j => j.Level == f.Level);
            if (f.RemoteOnly) q = q.Where(j => j.IsRemote);

            q = f.Sort switch
            {
                "salary" => q.OrderByDescending(j => j.SalaryMax),
                "deadline" => q.OrderBy(j => j.Deadline),
                "popular" => q.OrderByDescending(j => j.ApplicantCount),
                _ => q.OrderByDescending(j => j.PostedDate)
            };

            var list = q.ToList();
            f.Total = list.Count;
            if (f.Page < 1) f.Page = 1;
            f.Jobs = list.Skip((f.Page - 1) * f.PageSize).Take(f.PageSize).ToList();
            f.Categories = MockData.CategoryIcons.Select(c => c.Name).ToList();
            return View(f);
        }

        public IActionResult Details(int id)
        {
            var job = MockData.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null) return NotFound();
            var vm = new JobDetailsViewModel
            {
                Job = job,
                Company = MockData.Companies.First(c => c.Id == job.CompanyId),
                Similar = MockData.Jobs.Where(j => j.Id != id && (j.Category == job.Category || j.Type == job.Type) && j.Status == "Active").Take(3).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Apply(int id)
        {
            var job = MockData.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null) return NotFound();
            ViewBag.Profile = MockData.Profile;
            return View(job);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Apply(int id, string? coverLetter)
        {
            TempData["Success"] = "Application submitted! Track it from My Applications.";
            return RedirectToAction("Applications", "JobSeeker");
        }
    }
}
