using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;

namespace ItransitionTemplates.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ItransitionTemplates.Models.User user = Session.GetObject<ItransitionTemplates.Models.User>(HttpContext, "userSession");

        TempData["username"] = user.Username;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
