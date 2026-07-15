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
        Models.User userSession = Auth.ValidateUserSession(HttpContext);
        TempData["topics"] = System.Text.Json.JsonSerializer.Serialize(topics);
        return View("CreateTemplate");
    }

    //Create a template
    [HttpPost("/template/create")]
    public async Task<ActionResult<Models.Template>> CreateTemplate([FromBody] Models.Template template) {
        if(template.TopicId <= 0) {
            TempData["errorMsg"] = "Define a topic for this template";
            return View("CreateTemplate");
        }

        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Login to complete this action", 401);
        }

        if(template == null) {
            return JsonResponse.Error("The tamplate is null");
        }

        Models.Template saved = await _TemplateService.AddTemplate(template);
        Models.Admin admin = new Models.Admin();
        admin.UserId = userSession.UserId;
        admin.TemplateId = saved.TemplateId;
        Models.Admin adminSaved = await _AdminService.AddAdmin(admin);

        if(saved == null || adminSaved == null) {
            return JsonResponse.Error("An error has ocurred, try again");
        }

        return Ok(saved);
    }

    //Get all the templates (from the newest to the oldest ones)
    [HttpGet("/template/templates")]
    public async Task<IActionResult> GetTemplatesAndAdmins([FromQuery] int page, [FromQuery] int limit = 10) {
        Models.Template[] templates = await _TemplateService.GetLatestTemplatesWithAdmins(page, limit);
        return JsonResponse.Ok(templates);
    }

    //Get templates by the user Id
    [HttpGet("/template/template/user")]
    public async Task<IActionResult> GetTemplatesByUserId([FromQuery] int page, [FromQuery] int limit, [FromQuery] ulong userId) {
        Models.Template[] templates = await _TemplateService.GetTemplatesByUserId(page, limit, userId);
        return JsonResponse.Ok(templates);
    }

    //Get template by Id (just the view), this view is going to be for the forms (answers)
    [HttpGet("/template/template")]
    public IActionResult GetTemplateView() {
        Models.User user = Auth.ValidateUserSession(HttpContext);
        TempData["userEmail"] = user.Email;
        TempData["userId"] = user.UserId;
        return View("TemplateView");
    }

    //Get template by ID
    [HttpGet("/template/get-template")]
    public async Task<IActionResult> GetTemplate([FromQuery] ulong templateId) {
        Models.Template template = await _TemplateService.GetTemplateById(templateId);

        if(template == null) {
            return JsonResponse.NotFound("Resource not found");
        }

        return Ok(template);
    }

    //Update a template
    [HttpPut("/template/update")]
    public async Task<ActionResult> UpdateTemplate([FromQuery] ulong templateId, [FromBody] Models.Template template) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Login to complete this action", 401);
        }

        try {
            int result = await _TemplateService.UpdateTemplate(templateId, template);
            if(result == 404) {
                return JsonResponse.NotFound("The tamplate was not found");
            } else if(result == 500) {
                return JsonResponse.Error("This template could not be updated");
            }
        } catch (Exception err) {
            if(err.ToString().Contains("cannot be tracked because another instance with the key value")) {
                return JsonResponse.Error("There is a entity with this value");
            }
        }

        return JsonResponse.Ok("Template updated successfully");
    }

    //It is used to give a like to certain template
    //**** It requires Authentication
    [HttpGet("/template/like")]
    public async Task<IActionResult> LikeAction([FromQuery] ulong userId, [FromQuery] ulong templateId, [FromQuery] string action) {

        //Ensure the user has a session
        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Login to complete this action", 401);
        }

        Like[] actionCompleted = await _TemplateService.LikeAction(userId, templateId, action);

        if(actionCompleted != null) {
            return JsonResponse.Ok(actionCompleted.Length);
        }

        return JsonResponse.Error("We could not complete this action");
    }

    //It return the number of likes of a given template
    [HttpGet("/template/likes")]
    public async Task<IActionResult> GetTemplateLikes([FromQuery] ulong templateId) {
        Models.Template template = await _TemplateService.GetTemplateById(templateId);

        if(template != null) {
            return JsonResponse.Ok(template.Likes);
        }

        return JsonResponse.NotFound("The template does not exists");
    }

    [HttpDelete("/template/delete")]
    public async Task<IActionResult> DeleteTemplate([FromQuery] ulong templateId) {
        try {
            Models.User? userSession = Auth.ValidateSession(HttpContext);

            if(userSession == null) {
                return JsonResponse.Error("Login to complete this action", 401);
            }

            int n = await _TemplateService.DeleteTemplate(templateId);

            if(n == 200) {
                return JsonResponse.Ok("Template deleted successfully");
            } else {
                return JsonResponse.Error("This action could not be done");
            }
        } catch(Exception err) {
            return JsonResponse.Error("This action could not be done");
        }
    }

    [HttpGet("/template/get-by-query")]
    public async Task<IActionResult> GetTemplatesByQuery([FromQuery] string text) {
        Models.Template[] templates = await _TemplateService.GetTemplatesByQuery(text);

        if(templates.Length == 0) {
            return JsonResponse.NotFound("No templates were found try other terms");
        }

        return JsonResponse.Ok(templates);
    }

}
