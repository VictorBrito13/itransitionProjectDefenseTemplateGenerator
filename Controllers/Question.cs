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

        public QuestionController(Services.Question.IQuestion question, Services.QuestionOption.IQuestionOption questionOptionService) {
            _QuestionService = question;
            _QuestionOptionService = questionOptionService;
        }

        [HttpPost("/question/add")]
        public async Task<ActionResult<Models.Question[]>> AddQuestions([FromBody] QuestionAndOptions questionAndOptions) {
            if (questionAndOptions == null || questionAndOptions.questions.Length == 0)
            {
                return JsonResponse.Error("There are no questions for this form");
            }
            
            Models.Question[] saved = await _QuestionService.AddQuestions(questionAndOptions.questions);
            Models.QuestionOption[] optionsSaved = await _QuestionOptionService.AddOptions(questionAndOptions.questionOptions);

            if(saved == null) {
                return JsonResponse.Error("We could not add questions");
            }

            if(optionsSaved == null) {
                return JsonResponse.Error("We could not add options to the questions");
            }

            return JsonResponse.Ok("success");
        }
    }
}
