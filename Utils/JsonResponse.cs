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
            return new JsonResult(new { errorMsg = message, status = statusCode }) { StatusCode = statusCode };
        }

        public static JsonResult NotFound(string message)
        {
            return new JsonResult(new { errorMsg = message }) { StatusCode = 404 };
        }
    }
}
