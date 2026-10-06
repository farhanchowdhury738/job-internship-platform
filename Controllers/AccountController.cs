using JobInternshipPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobInternshipPlatform.Controllers
{
    /// <summary>
    /// UI-only authentication screens. TODO (back end): validate credentials, issue JWT, hash passwords.
    /// For now the selected role simply decides which dashboard you are redirected to.
    /// </summary>
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? role) => View(new LoginViewModel { Role = role ?? "JobSeeker" });

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Please enter your email and password.");
                return View(model);
            }
            TempData["Success"] = "Welcome back! (demo login - no back end yet)";
            return RedirectToRole(model.Role);
        }

        [HttpGet]
        public IActionResult Register(string? role) => View(new RegisterViewModel { Role = role ?? "JobSeeker" });

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(model);
            }
            TempData["Success"] = "Account created successfully! (demo registration)";
            return RedirectToRole(model.Role);
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            TempData["Success"] = "If that email exists, a reset link has been sent.";
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            TempData["Success"] = "You have been signed out.";
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToRole(string role) => role switch
        {
            "Recruiter" => RedirectToAction("Dashboard", "Recruiter"),
            "Admin" => RedirectToAction("Dashboard", "Admin"),
            _ => RedirectToAction("Dashboard", "JobSeeker")
        };
    }
}
