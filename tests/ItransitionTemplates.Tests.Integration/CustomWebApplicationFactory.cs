using System.Net;
using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ItransitionTemplates.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Remove the existing MySQL DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDBContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Also remove any DbContext registrations for ApplicationDBContext
            var dbDescriptors = services.Where(
                d => d.ServiceType == typeof(ApplicationDBContext)).ToList();
            foreach (var d in dbDescriptors)
            {
                services.Remove(d);
            }

            // Add InMemory DbContext
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            // Ensure the database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            db.Database.EnsureCreated();

            // Seed default topics needed by templates
            SeedData(db);
        });
    }

    private static void SeedData(ApplicationDBContext db)
    {
        // Seed topics that templates reference
        if (!db.Topics.Any())
        {
            db.Topics.AddRange(
                new Topic { TopicId = 1, Name = "Education" },
                new Topic { TopicId = 2, Name = "Survey" },
                new Topic { TopicId = 3, Name = "Feedback" }
            );
            db.SaveChanges();
        }
    }

    /// <summary>
    /// Creates a new scope to seed data into the in-memory database.
    /// </summary>
    public async Task SeedAsync(Action<ApplicationDBContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        seedAction(db);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Creates an HttpClient that has an authenticated session by
    /// seeding a user and performing a login POST.
    /// </summary>
    public async Task<HttpClient> CreateAuthenticatedClientAsync(
        string email = "testuser@test.com",
        string username = "testuser",
        string password = "TestPassword123")
    {
        // Seed a user with hashed password
        await SeedAsync(db =>
        {
            if (!db.Users.Any(u => u.Email == email))
            {
                db.Users.Add(new User
                {
                    Username = username,
                    Email = email,
                    Password = HashText.GetHashString(password)
                });
            }
        });

        var client = CreateClient();

        // Perform login to establish session
        var loginContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Email", email),
            new KeyValuePair<string, string>("Password", password)
        });

        var loginResponse = await client.PostAsync("/user/log-in", loginContent);

        // Login should redirect (302) on success
        return client;
    }
}
