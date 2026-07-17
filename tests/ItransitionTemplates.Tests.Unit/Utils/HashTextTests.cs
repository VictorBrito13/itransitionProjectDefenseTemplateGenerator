using ItransitionTemplates.Utils;
using Xunit;

namespace ItransitionTemplates.Tests.Unit.Utils
{
    public class HashTextTests
    {
        [Fact]
        public void GetHashString_KnownInput_CanBeVerified()
        {
            var input = "hello";

            var hash = HashText.GetHashString(input);

            Assert.True(HashText.VerifyHash(input, hash));
        }

        [Fact]
        public void GetHashString_DifferentInputs_ReturnDifferentHashes()
        {
            var hash1 = HashText.GetHashString("input1");
            var hash2 = HashText.GetHashString("input2");

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashString_EmptyString_CanBeVerified()
        {
            var hash = HashText.GetHashString("");

            Assert.True(HashText.VerifyHash("", hash));
        }

        [Fact]
        public void GetHashString_SameInput_ProducesDifferentHashesButBothVerify()
        {
            var input = "consistent";

            var hash1 = HashText.GetHashString(input);
            var hash2 = HashText.GetHashString(input);

            Assert.NotEqual(hash1, hash2);
            Assert.True(HashText.VerifyHash(input, hash1));
            Assert.True(HashText.VerifyHash(input, hash2));
        }

        [Fact]
        public void GetHashString_ReturnsBase64String()
        {
            var result = HashText.GetHashString("test");

            Assert.NotEmpty(result);
            var bytes = Convert.FromBase64String(result);
            Assert.Equal(48, bytes.Length); // 16 salt + 32 hash
        }

        [Fact]
        public void VerifyHash_WrongPassword_ReturnsFalse()
        {
            var hash = HashText.GetHashString("correct-password");

            Assert.False(HashText.VerifyHash("wrong-password", hash));
        }

        [Fact]
        public void VerifyHash_InvalidHash_ReturnsFalse()
        {
            Assert.False(HashText.VerifyHash("test", "not-a-valid-hash"));
        }
    }
}
