using JobInternshipPlatform.Data;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index(string? q, string? industry)
        {
            var list = MockData.Companies.Where(c => c.Status != "Suspended").AsEnumerable();
            if (!string.IsNullOrWhiteSpace(q)) list = list.Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(industry)) list = list.Where(c => c.Industry == industry);
            ViewBag.Q = q;
            ViewBag.Industry = industry;
            ViewBag.Industries = MockData.Companies.Select(c => c.Industry).Distinct().OrderBy(x => x).ToList();
            return View(list.ToList());
        }

        public IActionResult Details(int id)
        {
            var c = MockData.Companies.FirstOrDefault(x => x.Id == id);
            if (c == null) return NotFound();
            ViewBag.Jobs = MockData.Jobs.Where(j => j.CompanyId == id && j.Status == "Active").ToList();
            return View(c);
        }
    }
}
