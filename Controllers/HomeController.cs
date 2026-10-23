using HireLink.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireLink.Controllers;

public partial class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await _context.Jobs.Include(j => j.User)
            .OrderByDescending(j => j.PostedAt).ToListAsync();
        return View(jobs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var job = await _context.Jobs.Include(j => j.User)
            .FirstOrDefaultAsync(j => j.Id == id);
        if (job == null) return NotFound();
        return View(job);
    }
}
