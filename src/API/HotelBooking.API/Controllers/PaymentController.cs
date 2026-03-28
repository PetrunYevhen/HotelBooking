
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Commands.CompletePayment;
using Payments.Application.Contracts;
using Payments.Application.Queries.GetPaymentById;
using Payments.Domain.Entities;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentsModule _paymentsModule;

    public PaymentController(IPaymentsModule paymentManagementModule)
    {
        _paymentsModule = paymentManagementModule;
    }

    [HttpGet("{paymentId:guid}")]
    public async Task<ActionResult<Payment>> GetAsync(Guid paymentId)
    {
        var result = await _paymentsModule.ExecuteQueryAsync(new GetByIdQuery(paymentId));
        return Ok(result);
    }

    [HttpPost("{paymentId:guid}/complete")]
    public async Task<IActionResult> CompletePayment(Guid paymentId)
    {
        await _paymentsModule.ExecuteCommandAsync(new CompletePaymentCommand(paymentId));
        return Ok(); 
    }
}