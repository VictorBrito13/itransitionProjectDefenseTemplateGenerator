using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers
{
    public class ResponseController : Controller {

        private ItransitionTemplates.Services.Response.IResponse _responseService;
        private readonly ILogger<ResponseController> _logger;

        public ResponseController(ItransitionTemplates.Services.Response.IResponse responseService, ILogger<ResponseController> logger) {
            _responseService = responseService;
            _logger = logger;
        }

        [HttpPost("/response/add")]
        public async Task<ActionResult> SaveResponses([FromBody] Models.Response[] responses) {
            Models.User? userSession = Auth.ValidateSession(HttpContext);
            if (userSession == null) {
                return JsonResponse.Error("Please sign in to submit responses", 401);
            }

            foreach (var r in responses) {
                r.UserId = userSession.UserId;
            }

            try {
                await _responseService.AddResponses(responses);
                return JsonResponse.Ok("Responses saved successfully");
            } catch (ServiceException ex) {
                return JsonResponse.Error(ex.Message, ex.StatusCode);
            }
        }
    }
}
