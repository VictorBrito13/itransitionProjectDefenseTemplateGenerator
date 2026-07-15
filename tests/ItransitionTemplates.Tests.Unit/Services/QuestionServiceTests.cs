using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using QuestionService = ItransitionTemplates.Services.Question.Question;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class QuestionServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly QuestionService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<QuestionService> _logger;

        public QuestionServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<QuestionService>.Instance;
            _service = new QuestionService(_context, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<ItransitionTemplates.Models.Template> SeedTemplate()
        {
            var topic = new Topic { Name = "Test Topic" };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            var template = new ItransitionTemplates.Models.Template
            {
                Title = "Test Template",
                Description = "Description",
                TopicId = topic.TopicId
            };
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }

        [Fact]
        public async Task AddQuestions_ValidQuestions_ReturnsQuestions()
        {
            // Arrange
            var template = await SeedTemplate();
            var questions = new Question[]
            {
                new Question
                {
                    QuestionString = "What is your name?",
                    TemplateId = template.TemplateId,
                    QuestionType = QuestionType.singleLineString
                },
                new Question
                {
                    QuestionString = "Describe yourself",
                    TemplateId = template.TemplateId,
                    QuestionType = QuestionType.multipleLineText
                }
            };

            // Act
            var result = await _service.AddQuestions(questions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.All(result, q => Assert.NotEqual(0UL, q.QuestionId));
        }

        [Fact]
        public async Task AddQuestions_EmptyArray_ReturnsNull()
        {
            // Arrange
            var questions = Array.Empty<Question>();

            // Act
            var result = await _service.AddQuestions(questions);

            // Assert
            Assert.Null(result);
        }
    }
}
