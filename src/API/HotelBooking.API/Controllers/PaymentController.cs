
using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Commands.ConfirmPayment;
using Payments.Application.Contracts;
using Payments.Application.Queries.GetPaymenDetails;
using Payments.Application.Queries.GetPaymentByBookingId;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/payment")]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentsModule _paymentsModule;
    private readonly IBookingsModule _bookingsModule;

    public PaymentController(IPaymentsModule paymentsModule, IBookingsModule bookingsModule)
    {
        _paymentsModule = paymentsModule;
        _bookingsModule = bookingsModule;
    }

    [Authorize]
    [HttpGet("{paymentId:guid}")]
    [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid paymentId, CancellationToken cancellationToken)
    {
        var result = await _paymentsModule.ExecuteQueryAsync(new GetPaymentDetailsQuery(paymentId), cancellationToken);
        if (result is null) return NotFound();
        if (!await CanAccessBooking(result.BookingId, cancellationToken)) return Forbid();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("by-booking/{bookingId:guid}")]
    [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        if (!await CanAccessBooking(bookingId, cancellationToken)) return Forbid();
        var result = await _paymentsModule.ExecuteQueryAsync(new GetPaymentByBookingIdQuery(bookingId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPost("{paymentId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(Guid paymentId, [FromBody] ConfirmPaymentRequest? request, CancellationToken cancellationToken)
    {
        var payment = await _paymentsModule.ExecuteQueryAsync(new GetPaymentDetailsQuery(paymentId), cancellationToken);
        if (payment is null) return NotFound();
        if (!await CanAccessBooking(payment.BookingId, cancellationToken)) return Forbid();

        var result = await _paymentsModule.ExecuteCommandAsync(new ConfirmPaymentCommand(paymentId, request?.PaymentMethod), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return NoContent();
    }

    // [HttpPost("{id:guid}/fail")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Fail(Guid id, [FromBody] FailPaymentRequest request, CancellationToken cancellationToken)
    // {
    //     var result = await _paymentsModule.ExecuteCommandAsync(new FailPaymentCommand(id, request.Reason));
    //     if (result.IsFailure)
    //         return BadRequest(new { result.Error.Code, result.Error.Message });
    //     return NoContent();
    // }

    private async Task<bool> CanAccessBooking(Guid bookingId, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId)) return false;
        if (User.IsInRole("Admin")) return true;
        var bookingUserId = await _bookingsModule.ExecuteQueryAsync(new GetBookingUserIdQuery(bookingId), cancellationToken);
        return bookingUserId.HasValue && bookingUserId.Value == userId;
    }

    private bool TryGetCurrentUserId(out Guid userId) => Guid.TryParse(User.FindFirst("sub")?.Value, out userId);
}

public sealed record ConfirmPaymentRequest(string? PaymentMethod);
