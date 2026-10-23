using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HireLink.Controllers;

public partial class HomeController
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // The exception handler view intentionally exposes no exception details.
        _ = HttpContext.Features.Get<IExceptionHandlerFeature>();
        return View();
    }
}
