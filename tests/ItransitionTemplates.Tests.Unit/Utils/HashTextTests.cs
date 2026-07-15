using ItransitionTemplates.Utils;
using Xunit;

namespace ItransitionTemplates.Tests.Unit.Utils
{
    public class HashTextTests
    {
        [Fact]
        public void GetHashString_KnownInput_ReturnsExpectedHash()
        {
            // Arrange
            var input = "hello";
            // SHA256 of "hello" is well-known
            var expected = "2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824";

            // Act
            var result = HashText.GetHashString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetHashString_DifferentInputs_ReturnDifferentHashes()
        {
            // Arrange
            var hash1 = HashText.GetHashString("input1");
            var hash2 = HashText.GetHashString("input2");

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashString_EmptyString_ReturnsHash()
        {
            // Arrange
            var expected = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

            // Act
            var result = HashText.GetHashString("");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetHashString_SameInput_ReturnsSameHash()
        {
            // Arrange
            var input = "consistent";

            // Act
            var hash1 = HashText.GetHashString(input);
            var hash2 = HashText.GetHashString(input);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashString_ReturnsHexString()
        {
            // Act
            var result = HashText.GetHashString("test");

            // Assert
            Assert.Equal(64, result.Length); // SHA256 produces 32 bytes = 64 hex chars
            Assert.Matches("^[0-9a-f]+$", result);
        }
    }
}
