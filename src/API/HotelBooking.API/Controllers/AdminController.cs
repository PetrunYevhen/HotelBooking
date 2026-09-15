using Accommodations.Application.Command.Hotels.AssignHotelOwner;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetHotelsByOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Accommodations.Application.HotelierApplications;
using Accommodations.Domain.Entities.HotelierApplications;
using SharedKernel.Contracts;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public sealed class AdminController(IAccommodationsModule accommodations) : ControllerBase
{
    [HttpGet("hotelier-applications")]
    public async Task<IActionResult> GetHotelierApplications([FromQuery] string? status, CancellationToken cancellationToken) =>
        Ok(await accommodations.ExecuteQueryAsync(new GetHotelierApplicationsQuery(status), cancellationToken));

    [HttpPost("hotelier-applications/{applicationId:guid}/approve")]
    public async Task<IActionResult> ApproveHotelierApplication(Guid applicationId, CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out var adminId)) return Unauthorized();
        var result = await accommodations.ExecuteCommandAsync(new ReviewHotelierApplicationCommand(new HotelierApplicationId(applicationId), new AccountId(adminId), true, null) { IsAdmin = User.IsInRole("Admin") }, cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("hotelier-applications/{applicationId:guid}/reject")]
    public async Task<IActionResult> RejectHotelierApplication(Guid applicationId, RejectApplicationRequest request, CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out var adminId)) return Unauthorized();
        var result = await accommodations.ExecuteCommandAsync(new ReviewHotelierApplicationCommand(new HotelierApplicationId(applicationId), new AccountId(adminId), false, request.Reason) { IsAdmin = User.IsInRole("Admin") }, cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpGet("properties")]
    public async Task<IActionResult> GetProperties(CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out var adminId)) return Unauthorized();
        return Ok(await accommodations.ExecuteQueryAsync(new GetHotelsByOwnerQuery(adminId, true), cancellationToken));
    }

    [HttpPut("properties/{hotelId:guid}/owner")]
    public async Task<IActionResult> AssignOwner(Guid hotelId, AssignOwnerRequest request, CancellationToken cancellationToken)
    {
        var result = await accommodations.ExecuteCommandAsync(new AssignHotelOwnerCommand(hotelId, request.OwnerUserId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    private bool TryCurrentUser(out Guid userId) => Guid.TryParse(User.FindFirst("sub")?.Value, out userId);
}

public sealed record RejectApplicationRequest(string Reason);
public sealed record AssignOwnerRequest(Guid? OwnerUserId);
