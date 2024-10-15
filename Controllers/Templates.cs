using System.Text.Json;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class TemplateController : Controller {
    private readonly Services.Topic.ITopic _TopicService;
    private readonly Services.Template.ITemplate _TemplateService;
    private Services.Admin.IAdmin _AdminService;
    public TemplateController(Services.Topic.ITopic topicService, Services.Template.ITemplate templateService, Services.Admin.IAdmin _adminService) {
        _TopicService = topicService;
        _TemplateService = templateService;
        _AdminService = _adminService;
    }

    [HttpGet("/template/create")]
    public async Task<IActionResult> CreateTemplateView() {
        Models.Topic[] topics = await _TopicService.GetTopics();
        Models.User userSession = Session.GetObject<Models.User>(HttpContext, "userSession", false);
        TempData["topics"] = JsonSerializer.Serialize(topics);
        TempData["userId"] = userSession.UserId.ToString();
        Console.WriteLine(userSession.UserId);
        return View("CreateTemplate");
    }

    [HttpPost("/template/create")]
    public async Task<ActionResult<Models.Template>> createTemplate([FromBody] Models.Template template) {

        Models.User userSession = Session.GetObject<Models.User>(HttpContext, "userSession", true);

        if(userSession == null) {
            HttpContext.Response.Redirect("/user/log-in");
        }

        if(template == null) {
            return BadRequest(JsonSerializer.Serialize(new { errorMsg = "The tamplate is null" }));
        }

        Console.WriteLine(template.Questions);

        Models.Template saved = await _TemplateService.AddTemplate(template);
        Models.Admin admin = new Models.Admin();
        admin.UserId = userSession.UserId;
        admin.TemplateId = saved.TemplateId;
        Models.Admin adminSaved = await _AdminService.AddAdmin(admin);

        if(saved == null) {
            return BadRequest(JsonSerializer.Serialize(new { errorMsg = "An error has ocurred, try again" }));
        }

        return Ok(saved);
    }
}