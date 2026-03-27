using Microsoft.AspNetCore.Mvc;
using PaymentManagement.Application.Commands.CompletePayment;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Application.Queries.GetPaymentById;
using PaymentManagement.Domain.Entities;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentManagementModule _paymentManagementModule;

    public PaymentController(IPaymentManagementModule paymentManagementModule)
    {
        _paymentManagementModule = paymentManagementModule;
    }

    [HttpGet("{paymentId:guid}")]
    public async Task<ActionResult<Payment>> GetAsync(Guid paymentId)
    {
        var result = await _paymentManagementModule.ExecuteQueryAsync(new GetByIdQuery(paymentId));
        return Ok(result);
    }

    [HttpPost("{paymentId:guid}/complete")]
    public async Task<IActionResult> CompletePayment(Guid paymentId)
    {
        await _paymentManagementModule.ExecuteCommandAsync(new CompletePaymentCommand(paymentId));
        return Ok(); 
    }
}