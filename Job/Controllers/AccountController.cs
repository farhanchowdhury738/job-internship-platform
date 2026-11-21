
using Job.EF;
using Job.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UAParser;

namespace Job.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // Register Page
        // ==============================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ==============================
        // Register Submit
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email.Trim();

                var exists = await _context.Users.AnyAsync(u => u.Email == email);

                if (exists)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(model);
                }

                User user = new User
                {
                    FullName = model.FullName,
                    Email = email,
                    PasswordHash = model.Password,
                    Role = model.Role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // ==============================
        // Login Page
        // ==============================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // ==============================
        // Login Submit + Login Tracking
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Login(Login model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email.Trim();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user != null && user.PasswordHash == model.Password)
                {
                    // Generate Unique Login Session ID
                    string loginSessionId = Guid.NewGuid().ToString("N");

                    // Get IP Address
                    string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                    // Get User Agent
                    string? userAgent = Request.Headers["User-Agent"].ToString();

                    // Parse Browser, OS and Device
                    var parser = Parser.GetDefault();
                    var clientInfo = parser.Parse(userAgent ?? "");

                    string? browserName = clientInfo.UA.Family;
                    if (!string.IsNullOrWhiteSpace(clientInfo.UA.Major))
                        browserName += " " + clientInfo.UA.Major;

                    string? operatingSystem = clientInfo.OS.Family;
                    if (!string.IsNullOrWhiteSpace(clientInfo.OS.Major))
                        operatingSystem += " " + clientInfo.OS.Major;

                    string? deviceName = clientInfo.Device.Family;

                    // Create Login History
                    var loginHistory = new UserLoginHistory
                    {
                        UserId = user.UserId,
                        IpAddress = ipAddress,
                        LoginTime = DateTime.UtcNow,
                        UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
                        BrowserName = browserName,
                        OperatingSystem = operatingSystem,
                        DeviceName = deviceName,
                        SessionId = loginSessionId,
                        IsActive = true
                    };

                    _context.UserLoginHistory.Add(loginHistory);
                    await _context.SaveChangesAsync();

                    // Create Authentication Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("LoginSessionId", loginSessionId)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign In
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Return to Previous Page
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return LocalRedirect(returnUrl);

                    // Redirect Based on Role
                    if (user.Role == "admin")
                        return RedirectToAction("Dashboard", "Admin");
                    else if (user.Role == "job_seeker")
                        return RedirectToAction("Dashboard", "JobSeeker");
                    else if (user.Role == "company")
                        return RedirectToAction("Dashboard", "Company");
                }

                ModelState.AddModelError("", "Invalid Email or Password.");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // ==============================
        // Logout + Logout Tracking
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loginSessionId = User.FindFirst("LoginSessionId")?.Value;

            if (int.TryParse(userId, out int id) && !string.IsNullOrEmpty(loginSessionId))
            {
                var loginHistory = await _context.UserLoginHistory.FirstOrDefaultAsync(x =>
                    x.UserId == id && x.SessionId == loginSessionId && x.IsActive);

                if (loginHistory != null)
                {
                    loginHistory.LogoutTime = DateTime.UtcNow;
                    loginHistory.IsActive = false;
                    loginHistory.LogoutReason = "Manual";
                    await _context.SaveChangesAsync();
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
