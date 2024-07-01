using System.Net;
using L4D2PlayStats.FunctionApp.Errors;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.FunctionApp.Extensions;

public static class ErrorResultExtensions
{
    public static IActionResult ResponseMessageResult(this ErrorResult errorResult)
    {
        return errorResult.StatusCode switch
        {
            HttpStatusCode.BadRequest => new BadRequestObjectResult(errorResult),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(errorResult),
            _ => new BadRequestResult()
        };
    }
}