using System.Net;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetLogInView_ReturnsOkWithHtml()
    {
        // Act
        var response = await _client.GetAsync("/user/log-in");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    [Fact]
    public async Task GetSignUpView_ReturnsOkWithHtml()
    {
        // Act
        var response = await _client.GetAsync("/user/sign-up");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    [Fact]
    public async Task SignUp_ValidUser_RedirectsToLogin()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Username", "newuser"),
            new KeyValuePair<string, string>("Email", "newuser@test.com"),
            new KeyValuePair<string, string>("Password", "NewPassword123")
        });

        // Use a client that does NOT follow redirects so we can inspect the 302
        var noRedirectClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await noRedirectClient.PostAsync("/user/sign-up", formContent);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/user/log-in", response.Headers.Location?.ToString() ?? "");
    }

    [Fact]
    public async Task LogIn_ValidUser_RedirectsToHome()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        // Seed a user with hashed password
        await _factory.SeedAsync(db =>
        {
            if (!db.Users.Any(u => u.Email == "loginuser@test.com"))
            {
                db.Users.Add(new User
                {
                    Username = "loginuser",
                    Email = "loginuser@test.com",
                    Password = HashText.GetHashString("LoginPass123")
                });
            }
        });

        var noRedirectClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Email", "loginuser@test.com"),
            new KeyValuePair<string, string>("Password", "LoginPass123")
        });

        // Act
        var response = await noRedirectClient.PostAsync("/user/log-in", formContent);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        // RedirectToAction("Index", "Home") resolves to "/" via the default route
        var location = response.Headers.Location?.ToString() ?? "";
        Assert.True(location == "/" || location.Contains("/Home"),
            $"Expected redirect to / or /Home but got '{location}'");
    }

    [Fact]
    public async Task LogIn_InvalidUser_ReturnsViewWithError()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Email", "nonexistent@test.com"),
            new KeyValuePair<string, string>("Password", "WrongPassword")
        });

        // Act
        var response = await _client.PostAsync("/user/log-in", formContent);

        // Assert — returns the login view (200) with error message in TempData
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    [Fact]
    public async Task GetUserByUsername_NonExistent_ReturnsNotFound()
    {
        // Arrange — InMemory DB does not support FromSqlRaw (MySQL MATCH...AGAINST),
        // so the controller's catch block returns 404 for any query
        await _factory.InitializeDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/user/get-by-username?username=unknownuser");

        // Assert — the service catches the exception and returns null → controller returns 404
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("errorMsg", json);
    }

    [Fact]
    public async Task LogOut_RedirectsToLogin()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var noRedirectClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await noRedirectClient.GetAsync("/user/log-out");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/user/log-in", response.Headers.Location?.ToString() ?? "");
    }
}
