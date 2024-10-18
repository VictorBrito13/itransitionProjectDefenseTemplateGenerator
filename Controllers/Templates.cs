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
        Console.WriteLine(userSession.UserId);
        return View("CreateTemplate");
    }

    //Create a template
    [HttpPost("/template/create")]
    public async Task<ActionResult<Models.Template>> createTemplate([FromBody] Models.Template template) {

        if(template.TopicId <= 0) {
            TempData["errorMsg"] = "Define a topic for this template";
            return View("CreateTemplate");
        }

        Models.User userSession = Session.GetObject<Models.User>(HttpContext, "userSession", true);

        if(userSession == null) {
            HttpContext.Response.Redirect("/user/log-in");
        }

        if(template == null) {
            return BadRequest(JsonSerializer.Serialize(new { errorMsg = "The tamplate is null" }));
        }

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

    //Get all the templates
    [HttpGet("/template/templates")]
    public async Task<string> GetTemplatesAndAdmins([FromQuery] int page, [FromQuery] int limit = 10) {
        Models.Template[] templates = await _TemplateService.GetLatestTemplatesWithAdmins(page, limit);
        return JsonSerializer.Serialize(new { data = templates});
    }

    //Get templates by the user Id
    [HttpGet("/template/template/user")]
    public async Task<string> GetTemplatesByUserId([FromQuery] int page, [FromQuery] int limit, [FromQuery] ulong userId) {
        Console.WriteLine(userId);
        Models.Template[] templates = await _TemplateService.GetTemplatesByUserId(page, limit, userId);
        return JsonSerializer.Serialize(new { data = templates});
    }

    //Get templates by Id this view is going to be for the forms (answers)
    [HttpGet("/template/template")]
    public IActionResult GetTemplateView() {
        return View("TemplateView");
    }

    [HttpGet("/template/get-template")]
    public async Task<string> GetTemplate([FromQuery] ulong templateId) {
        Models.Template template = await _TemplateService.GetTemplateById(templateId);

        if(template == null) {
            return JsonSerializer.Serialize(new { errorMsg = "Resource not found"});
        } else {
            string templateString = JsonSerializer.Serialize(template);
            return templateString;
        }
    }


}