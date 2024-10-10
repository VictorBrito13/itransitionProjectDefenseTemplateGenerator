using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class TemplateController : Controller {

    [HttpGet("/template/create")]
    public IActionResult CreateTemplate() {
        return View();
    }
}