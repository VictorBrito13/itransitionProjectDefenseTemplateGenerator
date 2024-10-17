using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using System.Text.Json;

namespace ItransitionTemplates.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Services.Template.ITemplate _TemplateService;

    public HomeController(ILogger<HomeController> logger, Services.Template.ITemplate templateService)
    {
        _logger = logger;
        _TemplateService = templateService;
    }

    public IActionResult Index()
    {
        ItransitionTemplates.Models.User user = Session.GetObject<ItransitionTemplates.Models.User>(HttpContext, "userSession", false);
        TempData["username"] = user.Username;
        TempData["userId"] = user.UserId.ToString();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
