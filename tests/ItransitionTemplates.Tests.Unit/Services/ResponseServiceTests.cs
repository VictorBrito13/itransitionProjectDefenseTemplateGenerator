using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using ItransitionTemplates.Utils;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ResponseService = ItransitionTemplates.Services.Response.Response;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class ResponseServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly ResponseService _service;

        public ResponseServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _service = new ResponseService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<(ItransitionTemplates.Models.User user, Question question)> SeedUserAndQuestion()
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

            var user = new ItransitionTemplates.Models.User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = HashText.GetHashString("password")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var question = new Question
            {
                QuestionString = "What is your name?",
                TemplateId = template.TemplateId,
                QuestionType = QuestionType.singleLineString
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            // Detach the question to avoid navigation fixup issues with array-typed Responses collection
            _context.Entry(question).State = EntityState.Detached;

            return (user, question);
        }

        [Fact]
        public async Task AddResponses_ValidResponses_SavesSuccessfully()
        {
            // Arrange
            var (user, question) = await SeedUserAndQuestion();
            var responses = new ItransitionTemplates.Models.Response[]
            {
                new ItransitionTemplates.Models.Response
                {
                    ResponseString = "John",
                    UserId = user.UserId,
                    QuestionId = question.QuestionId
                }
            };

            // Act
            await _service.AddResponses(responses);

            // Assert
            var saved = await _context.Responses.FirstOrDefaultAsync(r => r.QuestionId == question.QuestionId);
            Assert.NotNull(saved);
            Assert.Equal("John", saved.ResponseString);
        }

        [Fact]
        public async Task AddResponses_EmptyArray_ThrowsServiceException()
        {
            // Arrange
            var responses = Array.Empty<ItransitionTemplates.Models.Response>();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.AddResponses(responses));
            Assert.Equal(ServiceErrorCode.Database, ex.ErrorCode);
        }
    }
}
