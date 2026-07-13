using Accommodations.Application.Command.Pricing.SetRoomPricing;
using Accommodations.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/pricing")]
public class PricingController : ControllerBase
{
    private readonly IAccommodationsModule _accommodationsModule;

    public PricingController(IAccommodationsModule accommodationsModule)
        => _accommodationsModule = accommodationsModule;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetRoomPricing(
        [FromBody] SetRoomPricingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(
            new SetRoomPricingCommand(
                request.RoomId,
                request.Price,
                request.Currency,
                request.ValidFrom,
                request.ValidTo),
            cancellationToken);

        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return StatusCode(StatusCodes.Status201Created);
    }
    // [HttpPost("{id:guid}/deactivate")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    // {
    //     var result = await _accommodationsModule
    //         .ExecuteCommandAsync(new DeactivatePricingCommand(id));
    //
    //     if (result.IsFailure)
    //         return BadRequest(new { result.Error.Code, result.Error.Message });
    //
    //     return NoContent();
    // }
}

public record SetRoomPricingRequest(
    Guid RoomId,
    decimal Price,
    string Currency,
    DateTime ValidFrom,
    DateTime ValidTo);
