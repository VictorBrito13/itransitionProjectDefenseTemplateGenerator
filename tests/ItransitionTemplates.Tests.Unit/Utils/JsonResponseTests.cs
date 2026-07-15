using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ItransitionTemplates.Tests.Unit.Utils
{
    public class JsonResponseTests
    {
        [Fact]
        public void Ok_ReturnsJsonResultWith200()
        {
            // Arrange
            var data = new { Name = "Test", Value = 42 };

            // Act
            var result = JsonResponse.Ok(data);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Ok_WrapsDataInDataProperty()
        {
            // Arrange
            var data = "test data";

            // Act
            var result = JsonResponse.Ok(data);

            // Assert
            var value = result.Value;
            Assert.NotNull(value);
            // The value is an anonymous object with a 'data' property
            var valueType = value.GetType();
            var dataProp = valueType.GetProperty("data");
            Assert.NotNull(dataProp);
            Assert.Equal("test data", dataProp.GetValue(value));
        }

        [Fact]
        public void Error_ReturnsJsonResultWithStatusCode()
        {
            // Arrange
            var message = "Something went wrong";

            // Act
            var result = JsonResponse.Error(message, 400);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Error_ContainsErrorMessage()
        {
            // Arrange
            var message = "Validation failed";

            // Act
            var result = JsonResponse.Error(message);

            // Assert
            var value = result.Value;
            Assert.NotNull(value);
            var valueType = value.GetType();
            var errorMsgProp = valueType.GetProperty("errorMsg");
            Assert.NotNull(errorMsgProp);
            Assert.Equal("Validation failed", errorMsgProp.GetValue(value));
        }

        [Fact]
        public void Error_DefaultStatusCode_Is400()
        {
            // Act
            var result = JsonResponse.Error("error");

            // Assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void NotFound_ReturnsJsonResultWith404()
        {
            // Arrange
            var message = "Resource not found";

            // Act
            var result = JsonResponse.NotFound(message);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void NotFound_ContainsErrorMessage()
        {
            // Arrange
            var message = "Template not found";

            // Act
            var result = JsonResponse.NotFound(message);

            // Assert
            var value = result.Value;
            Assert.NotNull(value);
            var valueType = value.GetType();
            var errorMsgProp = valueType.GetProperty("errorMsg");
            Assert.NotNull(errorMsgProp);
            Assert.Equal("Template not found", errorMsgProp.GetValue(value));
        }
    }
}
