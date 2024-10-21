using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers
{
    public class ResponseController : Controller {

        private ItransitionTemplates.Services.Response.IResponse _responseService;

        public ResponseController(ItransitionTemplates.Services.Response.IResponse responseService) {
            _responseService = responseService;
        }

        [HttpPost("/response/add")]
        public async Task<ActionResult> SaveResponses([FromBody] Models.Response[] responses) {
            Console.WriteLine(responses.Length);
            int n = await _responseService.AddResponses(responses);

            if(n >= 400 && n < 500) {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "This answers could not be saved" }));
            }

            return Ok(JsonSerializer.Serialize(new { data = "Responses saved successfully" }));
        }
    }
}