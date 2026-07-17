using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;

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
        _logger.LogInformation("DEBUG [Home.Index] Request received. CookiePresent={CookiePresent}, SessionId={SessionId}",
            HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Session"),
            HttpContext.Session.Id);

        ItransitionTemplates.Models.User user = Session.GetObject<ItransitionTemplates.Models.User>(HttpContext, "userSession");
        _logger.LogInformation("DEBUG [Home.Index] Session read. KeyExists={KeyExists}, RawJson={RawJson}, UserId={UserId}, Username={Username}, Email={Email}",
            HttpContext.Session.Keys.Contains("userSession"),
            HttpContext.Session.GetString("userSession"),
            user?.UserId,
            user?.Username,
            user?.Email);

        TempData["username"] = user?.Username;
        TempData["userId"] = user?.UserId.ToString();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
