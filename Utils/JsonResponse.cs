using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Utils
{
    public static class JsonResponse
    {
        public static JsonResult Ok(object data)
        {
            return new JsonResult(new { data }) { StatusCode = 200 };
        }

        public static JsonResult Error(string message, int statusCode = 400)
        {
            return new JsonResult(new
            {
                error = new
                {
                    code = statusCode,
                    message = message,
                    details = (string?)null
                }
            }) { StatusCode = statusCode };
        }

        public static JsonResult Error(string message, string details, int statusCode = 400)
        {
            return new JsonResult(new
            {
                error = new
                {
                    code = statusCode,
                    message = message,
                    details = details
                }
            }) { StatusCode = statusCode };
        }

        public static JsonResult NotFound(string message)
        {
            return new JsonResult(new
            {
                error = new
                {
                    code = 404,
                    message = message,
                    details = "NotFound"
                }
            }) { StatusCode = 404 };
        }
    }
}
