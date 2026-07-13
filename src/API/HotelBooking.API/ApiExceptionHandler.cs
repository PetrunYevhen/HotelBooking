using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace HotelBooking.API;

internal sealed class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var isBookingConflict = exception is PostgresException { SqlState: PostgresErrorCodes.ExclusionViolation };
        var isUniqueConflict = exception is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
        var status = isBookingConflict || isUniqueConflict
            ? StatusCodes.Status409Conflict
            : StatusCodes.Status500InternalServerError;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = status == StatusCodes.Status409Conflict
                ? "Request conflicts with current state"
                : "An unexpected error occurred",
            Detail = isBookingConflict
                ? "The room is already booked for the requested period."
                : status == StatusCodes.Status409Conflict
                    ? "A resource with the same unique identifier already exists."
                    : "The server could not complete the request.",
            Instance = httpContext.Request.Path
        };

        problem.Extensions["code"] = isBookingConflict
            ? "Booking.Overlap"
            : status == StatusCodes.Status409Conflict
                ? "Resource.Conflict"
                : "Server.Unexpected";

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(
            problem,
            options: null,
            contentType: "application/problem+json",
            cancellationToken);

        return true;
    }
}
