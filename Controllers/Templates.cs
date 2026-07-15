using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class TemplateController : Controller {
    private readonly Services.Topic.ITopic _TopicService;
    private readonly Services.Template.ITemplate _TemplateService;
    private Services.Admin.IAdmin _AdminService;
    private readonly ILogger<TemplateController> _logger;
    public TemplateController(Services.Topic.ITopic topicService, Services.Template.ITemplate templateService, Services.Admin.IAdmin _adminService, ILogger<TemplateController> logger) {
        _TopicService = topicService;
        _TemplateService = templateService;
        _AdminService = _adminService;
        _logger = logger;
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
            TempData["errorMsg"] = "Please select a topic for your template before saving";
            return View("CreateTemplate");
        }

        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Please sign in to create a template", 401);
        }

        if(template == null) {
            return JsonResponse.Error("Template data is missing — please check your input");
        }

        Models.Template saved = await _TemplateService.AddTemplate(template);
        Models.Admin admin = new Models.Admin();
        admin.UserId = userSession.UserId;
        admin.TemplateId = saved.TemplateId;
        Models.Admin adminSaved = await _AdminService.AddAdmin(admin);

        if(saved == null || adminSaved == null) {
            return JsonResponse.Error("Failed to save template — please try again");
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
            return JsonResponse.Error("Please sign in to update this template", 401);
        }

        try {
            int result = await _TemplateService.UpdateTemplate(templateId, template);
            if(result == 404) {
                return JsonResponse.NotFound("Template not found — it may have been deleted");
            } else if(result == 500) {
                return JsonResponse.Error("Failed to save changes — the template may have been modified by another user");
            }
        } catch (Exception err) {
            _logger.LogError(err, "Error updating template {TemplateId}", templateId);
            if(err.ToString().Contains("cannot be tracked because another instance with the key value")) {
                return JsonResponse.Error("A template with this title already exists");
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
            return JsonResponse.Error("Please sign in to like this template", 401);
        }

        Like[] actionCompleted = await _TemplateService.LikeAction(userId, templateId, action);

        _logger.LogInformation("Like action '{Action}' completed for template {TemplateId} by user {UserId}, result count: {Count}", action, templateId, userId, actionCompleted?.Length ?? 0);

        if(actionCompleted != null) {
            return JsonResponse.Ok(actionCompleted.Length);
        }

        return JsonResponse.Error("Failed to update like — please try again");
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
                return JsonResponse.Error("Please sign in to delete this template", 401);
            }

            int n = await _TemplateService.DeleteTemplate(templateId);

            if(n == 200) {
                return JsonResponse.Ok("Template deleted successfully");
            } else {
                return JsonResponse.Error("Failed to delete template — you may not have permission");
            }
        } catch(Exception err) {
            _logger.LogError(err, "Error deleting template {TemplateId}", templateId);
            return JsonResponse.Error("Failed to delete template — please try again");
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
