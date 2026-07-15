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
            int n = await _responseService.AddResponses(responses);

            if(n >= 400 && n < 500) {
                return JsonResponse.Error("This answers could not be saved");
            }

            return JsonResponse.Ok("Responses saved successfully");
        }
    }
}
