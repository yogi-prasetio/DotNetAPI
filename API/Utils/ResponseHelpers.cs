using Microsoft.AspNetCore.Mvc;
using System.Net;

internal static class ResponseHelpers
{
    public static ActionResult CreateResponse(HttpStatusCode statusCode, string message, Object? dataObject = null)
    {
        if (dataObject == null)
        {
            var result = new JsonResult(new
            {
                status = statusCode,
                message = message
            });
            return result;
        }
        else
        {
            var result = new JsonResult(new
            {
                status = statusCode,
                message = message,
                data = dataObject
            });
            return result;
        }
    }
}