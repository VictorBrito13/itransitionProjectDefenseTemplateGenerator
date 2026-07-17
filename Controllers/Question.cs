using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers {

    public class QuestionAndOptions {
        public Models.Question[] questions { get; set; }
        public Models.QuestionOption[] questionOptions { get; set; }
    }

    public class QuestionController : Controller {
        private Services.Question.IQuestion _QuestionService;
        private Services.QuestionOption.IQuestionOption _QuestionOptionService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(Services.Question.IQuestion question, Services.QuestionOption.IQuestionOption questionOptionService, ILogger<QuestionController> logger) {
            _QuestionService = question;
            _QuestionOptionService = questionOptionService;
            _logger = logger;
        }

        [HttpPost("/question/add")]
        public async Task<ActionResult<Models.Question[]>> AddQuestions([FromBody] QuestionAndOptions questionAndOptions) {
            Models.User? userSession = Auth.ValidateSession(HttpContext);
            if (userSession == null) {
                return JsonResponse.Error("Please sign in to add questions", 401);
            }

            if (questionAndOptions == null || questionAndOptions.questions == null || questionAndOptions.questions.Length == 0)
            {
                return JsonResponse.Error("No questions provided, please add at least one question");
            }
            
            Models.Question[] saved = await _QuestionService.AddQuestions(questionAndOptions.questions);
            Models.QuestionOption[] optionsSaved = await _QuestionOptionService.AddOptions(questionAndOptions.questionOptions);

            if(saved == null) {
                return JsonResponse.Error("Failed to save questions, please try again");
            }

            if(optionsSaved == null) {
                return JsonResponse.Error("Failed to save question options, please try again");
            }

            return JsonResponse.Ok("success");
        }
    }
}
