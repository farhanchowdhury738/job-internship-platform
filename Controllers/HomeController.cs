using JobInternshipPlatform.Data;
using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobInternshipPlatform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                FeaturedJobs = MockData.Jobs.Where(j => j.Status == "Active").OrderByDescending(j => j.PostedDate).Take(6).ToList(),
                TopCompanies = MockData.Companies.Where(c => c.Status == "Verified").Take(6).ToList(),
                Categories = MockData.CategoryIcons
                    .Select(c => (c.Name, c.Icon, MockData.Jobs.Count(j => j.Category == c.Name)))
                    .ToList()
            };
            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
