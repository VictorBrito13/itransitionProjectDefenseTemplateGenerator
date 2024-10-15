using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers
{
    public class AdminController : Controller {
        private Services.Admin.IAdmin _adminService;

        public AdminController(Services.Admin.IAdmin adminService) {
            _adminService = adminService;
        }

        [HttpPost("/admin/add")]
        public async Task<ActionResult<Models.Admin>> AddAdmin([FromBody] Models.Admin admin) {
            if(admin == null) {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "No admin was provided" }));
            }

            Console.WriteLine("template id form adimin");
            Console.WriteLine(admin.TemplateId);

            Models.Admin saved = await _adminService.AddAdmin(admin);

            return Ok(saved);
        }
    }
}