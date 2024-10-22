using System.Text.Json;
using ItransitionTemplates.Models;
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
    public async Task<ActionResult<Models.Template>> CreateTemplate([FromBody] Models.Template template) {
        Console.WriteLine(template);

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

        if(saved == null || adminSaved == null) {
            return BadRequest(JsonSerializer.Serialize(new { errorMsg = "An error has ocurred, try again" }));
        }

        return Ok(saved);
    }

    //Get all the templates (from the newest to the oldest ones)
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

    //Get template by Id (just the view), this view is going to be for the forms (answers)
    [HttpGet("/template/template")]
    public IActionResult GetTemplateView() {
        Models.User user = Session.GetObject<Models.User>(HttpContext, "userSession", false);
        TempData["userEmail"] = user.Email;
        TempData["userId"] = user.UserId;
        return View("TemplateView");
    }

    //Get template by ID
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

    //Update a template
    [HttpPut("/template/update")]
    public async Task<ActionResult> UpdateTemplate([FromQuery] ulong templateId, [FromBody] Models.Template template) {
        int result = await _TemplateService.UpdateTemplate(templateId, template);

        if(result == 404) {
            return NotFound(JsonSerializer.Serialize(new {errorMsg = "The tamplate was not found"}));
        } else if(result == 500) {
            return BadRequest(JsonSerializer.Serialize(new {errorMsg = "This template could not be updated"}));
        }

        return Ok(JsonSerializer.Serialize(new {data = "Template updated successfully"}));
    }

    //It is used to give a like to certain template
    [HttpGet("/template/like")]
    public async Task<IActionResult> LikeAction([FromQuery] ulong userId, [FromQuery] ulong templateId, [FromQuery] string action) {
        Like[] actionCompleted = await _TemplateService.LikeAction(userId, templateId, action);

        Console.WriteLine("Accion completada");
        Console.WriteLine(actionCompleted.Length);

        if(action == "like") {
            if(actionCompleted != null) {
                return Ok(JsonSerializer.Serialize(new { data = actionCompleted.Length }));
            } else {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "We could not complete this action" }));
            }
        } else {
            if(actionCompleted != null) {
                return Ok(JsonSerializer.Serialize(new { data = actionCompleted.Length }));
            } else {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "We could not complete this action" }));
            }
        }
    }

    //It return the number of likes of a given template
    [HttpGet("/template/likes")]
    public async Task<IActionResult> GetTemplateLikes([FromQuery] ulong templateId) {
        Console.WriteLine(templateId);
        Models.Template template = await _TemplateService.GetTemplateById(templateId);

        //Exists the tempalte
        if(template != null) {
            return Ok(JsonSerializer.Serialize(new { data = template.Likes }));
        }

        return NotFound(JsonSerializer.Serialize( new { errorMsg = "The template does not exists" }));
    }


}