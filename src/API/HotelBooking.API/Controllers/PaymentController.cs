
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Commands.CompletePayment;
using Payments.Application.Contracts;
using Payments.Application.Queries.GetPaymenDetails;
using Payments.Domain.Entities;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/payment")]
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
        var result = await _paymentsModule.ExecuteQueryAsync(new GetPaymentDetailsQuery(paymentId));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{paymentId:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid paymentId, [FromBody] CompletePaymentRequest request, CancellationToken cancellationToken)
    {
        var result = await _paymentsModule.ExecuteCommandAsync(new CompletePaymentCommand(paymentId, request.ExternalTransactionId));
        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Message });
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

public sealed record CompletePaymentRequest(string ExternalTransactionId);
