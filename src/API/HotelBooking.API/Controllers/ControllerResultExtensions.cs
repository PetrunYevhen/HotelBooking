using BuildingBlock.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

internal static class ControllerResultExtensions
{
    internal static IActionResult ToProblem(this ControllerBase controller, Error error)
    {
        var status = GetStatusCode(error.Code);
        var problem = new ProblemDetails
        {
            Status = status,
            Title = GetTitle(status),
            Detail = error.Message,
            Instance = controller.HttpContext.Request.Path
        };

        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem)
        {
            StatusCode = status,
            ContentTypes = { "application/problem+json" }
        };
    }

    private static int GetStatusCode(string code)
    {
        if (code.EndsWith(".NotFound", StringComparison.OrdinalIgnoreCase))
            return StatusCodes.Status404NotFound;

        if (code.EndsWith(".Unauthorized", StringComparison.OrdinalIgnoreCase))
            return StatusCodes.Status403Forbidden;

        if (code.Contains("Overlap", StringComparison.OrdinalIgnoreCase) ||
            code.Contains("InvalidState", StringComparison.OrdinalIgnoreCase) ||
            code.Contains("Already", StringComparison.OrdinalIgnoreCase))
            return StatusCodes.Status409Conflict;

        if (code.Contains("Gateway", StringComparison.OrdinalIgnoreCase))
            return StatusCodes.Status502BadGateway;

        return StatusCodes.Status400BadRequest;
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => "Resource not found",
        StatusCodes.Status403Forbidden => "Access forbidden",
        StatusCodes.Status409Conflict => "Request conflicts with current state",
        StatusCodes.Status502BadGateway => "External payment service error",
        _ => "Request validation failed"
    };
}
