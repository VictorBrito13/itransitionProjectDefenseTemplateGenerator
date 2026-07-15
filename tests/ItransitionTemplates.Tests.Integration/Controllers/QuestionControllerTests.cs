using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Models;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class QuestionControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.Preserve,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    // Request serialization: no ReferenceHandler.Preserve (server model binder
    // doesn't expect $id/$ref metadata in incoming payloads)
    private static readonly JsonSerializerOptions RequestJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public QuestionControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddQuestions_ValidPayload_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        // Seed a template and a question for the options to reference
        await _factory.SeedAsync(db =>
        {
            if (!db.Templates.Any(t => t.TemplateId == 600))
            {
                db.Templates.Add(new Template
                {
                    TemplateId = 600,
                    Title = "Question Test Template",
                    Description = "Template for question tests",
                    TopicId = 1,
                    IsPublic = true,
                    Image_url = "default.png"
                });
            }
        });

        // Seed a question that options can reference
        ulong seededQuestionId = 0;
        await _factory.SeedAsync(db =>
        {
            if (!db.Questions.Any(q => q.QuestionId == 600))
            {
                var q = new Question
                {
                    QuestionId = 600,
                    QuestionString = "Seeded question",
                    TemplateId = 600,
                    QuestionType = QuestionType.singleLineString
                };
                db.Questions.Add(q);
                db.SaveChanges();
                seededQuestionId = q.QuestionId;
            }
            else
            {
                seededQuestionId = 600;
            }
        });

        var payload = new
        {
            questions = new[]
            {
                new
                {
                    questionString = "What is your name?",
                    templateId = (ulong)600,
                    questionType = 0  // singleLineString enum value
                }
            },
            questionOptions = new[]
            {
                new
                {
                    option = "Option A",
                    questionId = seededQuestionId
                },
                new
                {
                    option = "Option B",
                    questionId = seededQuestionId
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, RequestJsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/question/add", content);

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.True(response.StatusCode == HttpStatusCode.OK,
            $"Expected OK but got {response.StatusCode}. Body: {responseBody}");
        Assert.Contains("data", responseBody);
    }

    [Fact]
    public async Task AddQuestions_EmptyQuestions_ReturnsBadRequest()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        var payload = new
        {
            questions = new object[] { },
            questionOptions = new object[] { }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, RequestJsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/question/add", content);

        // Assert — controller returns error (400) when questions array is empty
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("errorMsg", json);
    }
}
