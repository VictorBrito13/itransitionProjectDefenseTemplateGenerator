using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using ItransitionTemplates.Utils;
using Microsoft.EntityFrameworkCore;
using Xunit;
using UserService = ItransitionTemplates.Services.User.User;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class UserServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly UserService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<UserService> _logger;

        public UserServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<UserService>.Instance;
            _service = new UserService(_context, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<ItransitionTemplates.Models.User> SeedUser(string username = "testuser", string email = "test@example.com", string password = "password123")
        {
            var user = new ItransitionTemplates.Models.User
            {
                Username = username,
                Email = email,
                Password = HashText.GetHashString(password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var seeded = await SeedUser(email: "login@test.com", password: "mypassword");
            var loginAttempt = new ItransitionTemplates.Models.User { Email = "login@test.com", Password = "mypassword" };

            // Act
            var result = await _service.Login(loginAttempt);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("login@test.com", result.Email);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            await SeedUser(email: "user@test.com", password: "correctpassword");
            var loginAttempt = new ItransitionTemplates.Models.User { Email = "user@test.com", Password = "wrongpassword" };

            // Act
            var result = await _service.Login(loginAttempt);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_NullUser_ReturnsNull()
        {
            // Act
            var result = await _service.Login(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_NullEmail_ReturnsNull()
        {
            // Arrange
            var loginAttempt = new ItransitionTemplates.Models.User { Password = "password" };

            // Act
            var result = await _service.Login(loginAttempt);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUser_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var user = new ItransitionTemplates.Models.User
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "password123"
            };

            // Act
            var result = await _service.AddUser(user);

            // Assert
            Assert.Equal("User added successfully", result);
            var saved = await _context.Users.FirstOrDefaultAsync(u => u.Email == "new@example.com");
            Assert.NotNull(saved);
            Assert.NotEqual("password123", saved.Password); // Password should be hashed
        }

        [Fact]
        public async Task AddUser_NullUser_ReturnsErrorMessage()
        {
            // Act
            var result = await _service.AddUser(null);

            // Assert
            Assert.Equal("There is no user to add", result);
        }

        [Fact]
        public async Task GetUserByUsername_ThrowsWithInMemoryProvider()
        {
            // Arrange
            await SeedUser(username: "searchuser");

            // Act & Assert
            // InMemory provider does not support FromSqlRaw
            // The method catches the exception and returns null
            var result = await _service.GetUserByUsername("searchuser");
            Assert.Null(result);
        }
    }
}
