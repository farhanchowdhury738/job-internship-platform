using System.Security.Claims;
using HireLink.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// TEMPORARY (local development only): IPasswordHasher is intentionally not registered,
// because passwords are currently stored and compared in plain text by AccountController.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "HireLink.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        // Revalidate the cookie against the database so that a deactivated
        // account is signed out and a demoted administrator cannot keep stale
        // Admin access from an existing cookie.
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

                var userIdValue = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdValue, out var userId))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }

                var account = await db.Users.AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.Name, u.Role, u.IsActive })
                    .FirstOrDefaultAsync();

                if (account == null || !account.IsActive)
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }

                var cookieRole = context.Principal!.FindFirstValue(ClaimTypes.Role);
                var cookieName = context.Principal!.FindFirstValue(ClaimTypes.Name);
                if (!string.Equals(cookieRole, account.Role, StringComparison.Ordinal)
                    || !string.Equals(cookieName, account.Name, StringComparison.Ordinal))
                {
                    var identity = context.Principal!.Identity as ClaimsIdentity;
                    if (identity is null)
                    {
                        context.RejectPrincipal();
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return;
                    }

                    var roleClaim = identity.FindFirst(ClaimTypes.Role);
                    if (roleClaim != null) identity.RemoveClaim(roleClaim);
                    identity.AddClaim(new Claim(ClaimTypes.Role, account.Role));

                    var nameClaim = identity.FindFirst(ClaimTypes.Name);
                    if (nameClaim != null) identity.RemoveClaim(nameClaim);
                    identity.AddClaim(new Claim(ClaimTypes.Name, account.Name));

                    context.ReplacePrincipal(context.Principal);
                    context.ShouldRenew = true;
                }
            }
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
