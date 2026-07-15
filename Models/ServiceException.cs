namespace ItransitionTemplates.Models;

public enum ServiceErrorCode
{
    NotFound,
    Validation,
    Unauthorized,
    Forbidden,
    Conflict,
    Database,
    Unknown
}

public class ServiceException : Exception
{
    public ServiceErrorCode ErrorCode { get; }
    public int StatusCode { get; }

    public ServiceException(string message, ServiceErrorCode errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = GetStatusCode(errorCode);
    }

    public ServiceException(string message, ServiceErrorCode errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = GetStatusCode(errorCode);
    }

    private static int GetStatusCode(ServiceErrorCode code) => code switch
    {
        ServiceErrorCode.NotFound => 404,
        ServiceErrorCode.Validation => 400,
        ServiceErrorCode.Unauthorized => 401,
        ServiceErrorCode.Forbidden => 403,
        ServiceErrorCode.Conflict => 409,
        ServiceErrorCode.Database => 500,
        _ => 500
    };
}
