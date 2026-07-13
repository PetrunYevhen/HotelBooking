
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

    public PaymentController(IPaymentsModule paymentsModule)
    {
        _paymentsModule = paymentsModule;
    }

    [HttpGet("{paymentId:guid}")]
    [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid paymentId, CancellationToken cancellationToken)
    {
        var result = await _paymentsModule.ExecuteQueryAsync(new GetPaymentDetailsQuery(paymentId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-booking/{bookingId:guid}")]
    [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var result = await _paymentsModule.ExecuteQueryAsync(new GetPaymentByBookingIdQuery(bookingId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{paymentId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(Guid paymentId, [FromBody] ConfirmPaymentRequest? request, CancellationToken cancellationToken)
    {
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
    
}

public sealed record ConfirmPaymentRequest(string? PaymentMethod);
