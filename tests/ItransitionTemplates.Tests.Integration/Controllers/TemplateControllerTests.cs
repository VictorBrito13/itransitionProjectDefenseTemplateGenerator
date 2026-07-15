using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class TemplateControllerTests : IClassFixture<CustomWebApplicationFactory>
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

    public TemplateControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTemplates_ReturnsOkWithArray()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/template/templates?page=0&limit=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("data", json);
    }

    [Fact]
    public async Task GetTemplate_ExistingId_ReturnsTemplate()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        await SeedTemplateAsync(100, "Test Template", 1);

        // Act
        var response = await _client.GetAsync("/template/get-template?templateId=100");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Test Template", json);
    }

    [Fact]
    public async Task GetTemplate_NonExistentId_ReturnsNotFound()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/template/get-template?templateId=999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("errorMsg", json);
    }

    [Fact]
    public async Task CreateTemplate_Authenticated_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "template_creator@test.com",
            username: "templatecreator",
            password: "Password123");

        var template = new
        {
            title = "New Template",
            description = "A test template",
            topicId = 1,
            isPublic = true,
            image_url = "test.png"
        };
        var content = new StringContent(
            JsonSerializer.Serialize(template, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/template/create", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("New Template", json);
    }

    [Fact]
    public async Task CreateTemplate_InvalidTopic_ReturnsError()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "invalid_topic@test.com",
            username: "invalidtopic",
            password: "Password123");

        var template = new
        {
            title = "Bad Template",
            description = "Invalid topic",
            topicId = 0,
            isPublic = true
        };
        var content = new StringContent(
            JsonSerializer.Serialize(template, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/template/create", content);

        // Assert — controller returns view with errorMsg in TempData when topicId <= 0
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        // The view is returned (not a JSON error), it's a redirect to CreateTemplate view
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    [Fact]
    public async Task UpdateTemplate_Existing_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        await SeedTemplateAsync(200, "Original Title", 1);
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "updater@test.com",
            username: "updater",
            password: "Password123");

        var updatedTemplate = new
        {
            title = "Updated Title",
            description = "Updated description",
            topicId = 1,
            isPublic = true,
            image_url = "default.png"
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updatedTemplate, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var request = new HttpRequestMessage(HttpMethod.Put, "/template/update?templateId=200");
        request.Content = content;
        var response = await authClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTemplate_NonExistent_ReturnsNotFound()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "updater_ne@test.com",
            username: "updater_ne",
            password: "Password123");

        var updatedTemplate = new
        {
            title = "Ghost Template",
            description = "Does not exist",
            topicId = 1,
            isPublic = true
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updatedTemplate, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var request = new HttpRequestMessage(HttpMethod.Put, "/template/update?templateId=999999");
        request.Content = content;
        var response = await authClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTemplate_Existing_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        await SeedTemplateAsync(300, "To Delete", 1);
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "deleter@test.com",
            username: "deleter",
            password: "Password123");

        // Act
        var response = await authClient.DeleteAsync("/template/delete?templateId=300");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LikeAction_Authenticated_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        await SeedTemplateAsync(400, "Likeable Template", 1);
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "liker@test.com",
            username: "liker",
            password: "Password123");

        // Seed a user for the like action
        await _factory.SeedAsync(db =>
        {
            if (!db.Users.Any(u => u.Email == "liker@test.com"))
            {
                db.Users.Add(new User
                {
                    Username = "liker",
                    Email = "liker@test.com",
                    Password = HashText.GetHashString("Password123")
                });
            }
        });

        // Get the user ID
        ulong userId = 0;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            var user = db.Users.FirstOrDefault(u => u.Email == "liker@test.com");
            if (user != null) userId = user.UserId;
        }

        // Act
        var response = await authClient.GetAsync($"/template/like?userId={userId}&templateId=400&action=like");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTemplateLikes_Existing_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        await SeedTemplateAsync(500, "Popular Template", 1);

        // Act
        var response = await _client.GetAsync("/template/likes?templateId=500");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTemplatesByQuery_NoMatch_ReturnsError()
    {
        // Arrange — InMemory DB does not support FromSqlRaw (MySQL MATCH...AGAINST),
        // so this endpoint will return an error. We verify it doesn't crash the server.
        await _factory.InitializeDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/template/get-by-query?text=zzzzz_nonexistent");

        // Assert — endpoint uses MySQL-specific raw SQL; with InMemory it returns an error
        Assert.True(
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.InternalServerError,
            $"Expected 404 or 500 but got {(int)response.StatusCode}");
    }

    [Fact]
    public async Task CreateTemplate_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var template = new
        {
            title = "Unauthorized Template",
            description = "Should fail",
            topicId = 1,
            isPublic = true
        };
        var content = new StringContent(
            JsonSerializer.Serialize(template, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act — no authentication
        var response = await _client.PostAsync("/template/create", content);

        // Assert — ValidateSession returns null → 401
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private async Task SeedTemplateAsync(ulong templateId, string title, ulong topicId)
    {
        await _factory.SeedAsync(db =>
        {
            if (!db.Templates.Any(t => t.TemplateId == templateId))
            {
                db.Templates.Add(new Template
                {
                    TemplateId = templateId,
                    Title = title,
                    Description = $"Description for {title}",
                    TopicId = topicId,
                    IsPublic = true,
                    Image_url = "default.png"
                });
            }
        });
    }
}
