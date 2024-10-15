using System.Text.Json;
using ItransitionTemplates.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers {
    public class QuestionController : Controller {
        private Services.Question.IQuestion _QuestionService;

        public QuestionController(Services.Question.IQuestion question) {
            _QuestionService = question;
        }

        [HttpPost("/question/add")]
        public async Task<ActionResult<Models.Question[]>> AddQuestions([FromBody] Models.Question[] questions) {
            Console.WriteLine(questions);
            if (questions == null || questions.Length == 0)
            {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "There are no questions for this form" }));
            }
            
            Models.Question[] saved = await _QuestionService.AddQuestions(questions);

            if(saved == null) {
                return BadRequest(JsonSerializer.Serialize(new { errorMsg = "Whe could not add questions" }));
            }

            return Ok(JsonSerializer.Serialize(new { data= "success" }));
        }
    }
}