using System.Net;
using System.Text.Json;
using ItransitionTemplates.Models;

namespace ItransitionTemplates.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ServiceException ex)
        {
            _logger.LogWarning(ex, "Service error: {ErrorCode} - {Message}", ex.ErrorCode, ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.ErrorCode.ToString(), ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context,
                (int)HttpStatusCode.InternalServerError,
                "InternalError",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            error = new
            {
                code = statusCode,
                message = message,
                details = code
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
