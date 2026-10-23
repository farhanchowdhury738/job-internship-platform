using System.Security.Claims;
using HireLink.Data;
using HireLink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireLink.Controllers;

[Authorize(Roles = "Recruiter")]
public class RecruiterController : Controller
{
    private readonly AppDbContext _context;

    public RecruiterController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var jobs = await _context.Jobs.Where(j => j.UserId == userId)
            .OrderByDescending(j => j.PostedAt).ToListAsync();
        return View(jobs);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Job job)
    {
        ModelState.Remove(nameof(job.User));
        if (!ModelState.IsValid)
            return View(job);

        job.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        job.PostedAt = DateTime.Now;
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        if (job == null) return NotFound();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Job input)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        if (job == null) return NotFound();

        ModelState.Remove(nameof(input.User));
        if (!ModelState.IsValid) return View(input);

        job.Title = input.Title;
        job.Description = input.Description;
        job.Salary = input.Salary;
        job.Location = input.Location;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        if (job == null) return NotFound();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        if (job == null) return NotFound();

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
