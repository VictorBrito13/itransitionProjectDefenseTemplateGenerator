using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class TemplateController : Controller {
    private readonly Services.Topic.ITopic _TopicService;
    private readonly Services.Template.ITemplate _TemplateService;
    private Services.Admin.IAdmin _AdminService;
    private readonly ILogger<TemplateController> _logger;
    private readonly ApplicationDBContext _context;
    public TemplateController(Services.Topic.ITopic topicService, Services.Template.ITemplate templateService, Services.Admin.IAdmin _adminService, ILogger<TemplateController> logger, ApplicationDBContext context) {
        _TopicService = topicService;
        _TemplateService = templateService;
        _AdminService = _adminService;
        _logger = logger;
        _context = context;
    }

    [HttpGet("/template/create")]
    public async Task<IActionResult> CreateTemplateView() {
        _logger.LogInformation("DEBUG [CreateTemplateView] Request received. Path={Path}, Method={Method}", HttpContext.Request.Path, HttpContext.Request.Method);
        _logger.LogInformation("DEBUG [CreateTemplateView] Session cookie present: {CookiePresent}", HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Session"));
        _logger.LogInformation("DEBUG [CreateTemplateView] Session ID: {SessionId}", HttpContext.Session.Id);
        
        Models.User? userSession = Auth.ValidateSession(HttpContext);
        _logger.LogInformation("DEBUG [CreateTemplateView] ValidateSession returned: {IsNull}", userSession == null ? "null" : "valid user");
        
        if (userSession == null) {
            _logger.LogInformation("DEBUG [CreateTemplateView] Redirecting to /user/log-in");
            return new RedirectResult("/user/log-in", permanent: false, preserveMethod: true);
        }

        Models.Topic[] topics = await _TopicService.GetTopics();
        TempData["topics"] = System.Text.Json.JsonSerializer.Serialize(topics);
        return View("CreateTemplate");
    }

    [HttpPost("/template/create")]
    public async Task<ActionResult<Models.Template>> CreateTemplate([FromBody] Models.Template template) {
        if(template == null) {
            return JsonResponse.Error("Template data is missing, please check your input");
        }

        if(template.TopicId <= 0) {
            return JsonResponse.Error("Please select a topic for your template before saving");
        }

        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Please sign in to create a template", 401);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try {
            Models.Template saved = await _TemplateService.AddTemplate(template);
            if(saved == null) {
                await transaction.RollbackAsync();
                return JsonResponse.Error("Failed to save template, please try again");
            }

            Models.Admin admin = new Models.Admin();
            admin.UserId = userSession.UserId;
            admin.TemplateId = saved.TemplateId;
            Models.Admin adminSaved = await _AdminService.AddAdmin(admin);

            if(adminSaved == null) {
                await transaction.RollbackAsync();
                return JsonResponse.Error("Failed to save template, please try again");
            }

            await transaction.CommitAsync();
            return Ok(saved);
        } catch (Exception err) {
            await transaction.RollbackAsync();
            _logger.LogError(err, "Error creating template");
            return JsonResponse.Error("Failed to save template, please try again");
        }
    }

    [HttpGet("/template/templates")]
    public async Task<IActionResult> GetTemplatesAndAdmins([FromQuery] int page, [FromQuery] int limit = 10) {
        Models.Template[] templates = await _TemplateService.GetLatestTemplatesWithAdmins(page, limit);
        return JsonResponse.Ok(templates);
    }

    [HttpGet("/template/template/user")]
    public async Task<IActionResult> GetTemplatesByUserId([FromQuery] int page, [FromQuery] int limit, [FromQuery] ulong userId) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);
        if (userSession == null) {
            return JsonResponse.Error("Please sign in to view templates", 401);
        }

        Models.Template[] templates = await _TemplateService.GetTemplatesByUserId(page, limit, userId);

        if (userId != userSession.UserId) {
            templates = templates.Where(t => t.IsPublic).ToArray();
        }

        return JsonResponse.Ok(templates);
    }

    [HttpGet("/template/template")]
    public IActionResult GetTemplateView() {
        Models.User? user = Auth.ValidateUserSession(HttpContext);
        if (user == null) return new RedirectResult("/user/log-in", permanent: false, preserveMethod: true);
        TempData["userEmail"] = user.Email;
        TempData["userId"] = user.UserId;
        return View("TemplateView");
    }

    [HttpGet("/template/get-template")]
    public async Task<IActionResult> GetTemplate([FromQuery] ulong templateId) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);
        if (userSession == null) {
            return JsonResponse.Error("Please sign in to view this template", 401);
        }

        Models.Template template = await _TemplateService.GetTemplateById(templateId);

        if(template == null) {
            return JsonResponse.NotFound("Resource not found");
        }

        if (!template.IsPublic)
        {
            bool isAdmin = await _AdminService.IsUserAdmin(userSession.UserId, templateId);
            if (!isAdmin)
                return JsonResponse.NotFound("Resource not found");
        }

        return Ok(template);
    }

    [HttpPut("/template/update")]
    public async Task<ActionResult> UpdateTemplate([FromQuery] ulong templateId, [FromBody] Models.Template template) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Please sign in to update this template", 401);
        }

        bool isAdmin = await _AdminService.IsUserAdmin(userSession.UserId, templateId);
        if (!isAdmin) {
            return JsonResponse.Error("You are not authorized to update this template", 403);
        }

        try {
            await _TemplateService.UpdateTemplate(templateId, template);
        } catch (ServiceException ex) {
            return JsonResponse.Error(ex.Message, ex.StatusCode);
        } catch (Exception err) {
            _logger.LogError(err, "Error updating template {TemplateId}", templateId);
            return JsonResponse.Error("Failed to save changes, please try again");
        }

        return JsonResponse.Ok("Template updated successfully");
    }

    [HttpGet("/template/like")]
    public async Task<IActionResult> LikeAction([FromQuery] ulong templateId, [FromQuery] string action) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);

        if(userSession == null) {
            return JsonResponse.Error("Please sign in to like this template", 401);
        }

        Like[] actionCompleted = await _TemplateService.LikeAction(userSession.UserId, templateId, action);

        _logger.LogInformation("Like action '{Action}' completed for template {TemplateId} by user {UserId}, result count: {Count}", action, templateId, userSession.UserId, actionCompleted?.Length ?? 0);

        if(actionCompleted != null) {
            return JsonResponse.Ok(actionCompleted.Length);
        }

        return JsonResponse.Error("Failed to update like, please try again");
    }

    [HttpGet("/template/likes")]
    public async Task<IActionResult> GetTemplateLikes([FromQuery] ulong templateId) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);
        if (userSession == null) {
            return JsonResponse.Error("Please sign in to view likes", 401);
        }

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

            bool isAdmin = await _AdminService.IsUserAdmin(userSession.UserId, templateId);
            if (!isAdmin) {
                return JsonResponse.Error("You are not authorized to delete this template", 403);
            }

            await _TemplateService.DeleteTemplate(templateId);

            return JsonResponse.Ok("Template deleted successfully");
        } catch(ServiceException ex) {
            return JsonResponse.Error(ex.Message, ex.StatusCode);
        } catch(Exception err) {
            _logger.LogError(err, "Error deleting template {TemplateId}", templateId);
            return JsonResponse.Error("Failed to delete template, please try again");
        }
    }

    [HttpGet("/template/get-by-query")]
    public async Task<IActionResult> GetTemplatesByQuery([FromQuery] string text) {
        Models.User? userSession = Auth.ValidateSession(HttpContext);
        if (userSession == null) {
            return JsonResponse.Error("Please sign in to search templates", 401);
        }

        Models.Template[] templates = await _TemplateService.GetTemplatesByQuery(text);
        templates = templates.Where(t => t.IsPublic).ToArray();

        if(templates.Length == 0) {
            return JsonResponse.NotFound("No templates were found try other terms");
        }

        return JsonResponse.Ok(templates);
    }

}
