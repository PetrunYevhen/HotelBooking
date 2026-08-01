using System.Security.Claims;
using Accommodations.Application.Command.HotelAddOns.CreateHotelAddOn;
using Accommodations.Application.Command.HotelAddOns.SetHotelAddOnStatus;
using Accommodations.Application.Command.HotelAddOns.UpdateHotelAddOn;
using Accommodations.Application.Command.Hotels.AddHotelFacilities;
using Accommodations.Application.Command.Hotels.AssignHotelOwner;
using Accommodations.Application.Command.Hotels.RemoveHotelFacility;
using Accommodations.Application.Command.Hotels.SetPolicies;
using Accommodations.Application.Command.Rooms.AddRoomFacilities;
using Accommodations.Application.Command.Rooms.RemoveRoomFacility;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.HotelAddOns;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Accommodations.Application.Query.Hotels.GetHotelOwner;
using Accommodations.Application.Query.Hotels.GetHotelsByOwner;
using Accommodations.Application.Query.Rooms.GetRoomFacilities;
using Accommodations.Application.Query.Rooms.GetRoomsByHotelId;
using Bookings.Application.Command.CheckInBooking;
using Bookings.Application.Command.CheckOutBooking;
using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingById;
using Bookings.Application.Query.GetBookingsByHotelId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/hotelier")]
[Authorize(Roles = "Admin,Hotelier")]
public sealed class HotelierController(IAccommodationsModule accommodations, IBookingsModule bookings) : ControllerBase
{
    [HttpGet("hotels")]
    public async Task<IActionResult> GetHotels(CancellationToken cancellationToken)
    {
        if (!TryUser(out var userId)) return Unauthorized();
        // Admin can use this endpoint to see the currently unassigned legacy inventory too.
        var result = await accommodations.ExecuteQueryAsync(new GetHotelsByOwnerQuery(userId, User.IsInRole("Admin")), cancellationToken);
        return Ok(result);
    }

    [HttpPut("hotels/{hotelId:guid}/owner")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignOwner(Guid hotelId, [FromBody] AssignOwnerRequest request, CancellationToken cancellationToken)
    {
        var result = await accommodations.ExecuteCommandAsync(new AssignHotelOwnerCommand(hotelId, request.OwnerUserId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpGet("hotels/{hotelId:guid}/overview")]
    public async Task<IActionResult> Overview(Guid hotelId, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var today = DateTime.UtcNow.Date;
        var all = await bookings.ExecuteQueryAsync(new GetBookingsByHotelIdQuery(hotelId, today.AddDays(-1), today.AddDays(2)), cancellationToken);
        return Ok(new HotelierOverviewDto(
            all.Count(x => x.CheckInDate.Date == today && x.Status is "Pending" or "Confirmed"),
            all.Count(x => x.CheckOutDate.Date == today && x.Status is "Pending" or "Confirmed" or "CheckedIn"),
            all.Count(x => x.Status == "CheckedIn"),
            all.Count(x => x.CreatedAt.Date == today),
            all.Where(x => x.Status is "Pending" or "Confirmed").OrderBy(x => x.CheckInDate).Take(6).ToList()));
    }

    [HttpGet("hotels/{hotelId:guid}/bookings")]
    public async Task<IActionResult> GetBookings(Guid hotelId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? status, [FromQuery] Guid? roomId, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        return Ok(await bookings.ExecuteQueryAsync(new GetBookingsByHotelIdQuery(hotelId, from, to, status, roomId), cancellationToken));
    }

    [HttpGet("hotels/{hotelId:guid}/calendar")]
    public async Task<IActionResult> Calendar(Guid hotelId, [FromQuery] DateTime from, [FromQuery] int days = 14, CancellationToken cancellationToken = default)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        days = Math.Clamp(days, 1, 31);
        var to = from.Date.AddDays(days);
        var rooms = await accommodations.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(hotelId, from.Date), cancellationToken);
        var occupancy = await bookings.ExecuteQueryAsync(new GetBookingsByHotelIdQuery(hotelId, from.Date, to), cancellationToken);
        return Ok(new HotelierCalendarDto(rooms, occupancy.Where(x => x.Status is "Pending" or "Confirmed").ToList()));
    }

    [HttpPost("bookings/{bookingId:guid}/checkin")]
    public async Task<IActionResult> CheckIn(Guid bookingId, CancellationToken cancellationToken)
    {
        if (!await CanAccessBooking(bookingId, cancellationToken)) return Forbid();
        var result = await bookings.ExecuteCommandAsync(new CheckInBookingCommand(bookingId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("bookings/{bookingId:guid}/checkout")]
    public async Task<IActionResult> CheckOut(Guid bookingId, CancellationToken cancellationToken)
    {
        if (!await CanAccessBooking(bookingId, cancellationToken)) return Forbid();
        var result = await bookings.ExecuteCommandAsync(new CheckOutBookingCommand(bookingId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpGet("hotels/{hotelId:guid}/settings")]
    public async Task<IActionResult> GetSettings(Guid hotelId, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var rooms = await accommodations.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(hotelId, DateTime.UtcNow.Date), cancellationToken);
        var hotelFacilities = await accommodations.ExecuteQueryAsync(new GetHotelFacilitiesQuery(hotelId), cancellationToken);
        var addOns = await accommodations.ExecuteQueryAsync(new GetHotelAddOnsQuery(hotelId), cancellationToken);
        var roomFacilities = new Dictionary<Guid, object>();
        foreach (var room in rooms) roomFacilities[room.RoomId] = await accommodations.ExecuteQueryAsync(new GetRoomFacilitiesQuery(room.RoomId), cancellationToken);
        return Ok(new { rooms, hotelFacilities, roomFacilities, addOns });
    }

    [HttpPost("hotels/{hotelId:guid}/amenities")]
    public async Task<IActionResult> AddHotelAmenity(Guid hotelId, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new AddHotelFacilitiesCommand { HotelId = hotelId, Facilities = facilities }, cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpDelete("hotels/{hotelId:guid}/amenities/{facilityId:guid}")]
    public async Task<IActionResult> RemoveHotelAmenity(Guid hotelId, Guid facilityId, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new RemoveHotelFacilityCommand(hotelId, facilityId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("hotels/{hotelId:guid}/rooms/{roomId:guid}/amenities")]
    public async Task<IActionResult> AddRoomAmenity(Guid hotelId, Guid roomId, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        if (!await IsHotelRoom(hotelId, roomId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new AddRoomFacilitiesCommand { RoomId = roomId, Facilities = facilities }, cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpDelete("hotels/{hotelId:guid}/rooms/{roomId:guid}/amenities/{facilityId:guid}")]
    public async Task<IActionResult> RemoveRoomAmenity(Guid hotelId, Guid roomId, Guid facilityId, CancellationToken cancellationToken)
    {
        if (!await IsHotelRoom(hotelId, roomId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new RemoveRoomFacilityCommand(roomId, facilityId), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPut("hotels/{hotelId:guid}/policies")]
    public async Task<IActionResult> SetPolicies(Guid hotelId, [FromBody] SetHotelPoliciesCommand request, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new SetHotelPoliciesCommand(hotelId, request.CancellationPolicyType, request.DeadlineDays, request.PercentagePenalty, request.PetPolicy, request.SmokingPolicy, request.CheckOutHoursPolicy), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("hotels/{hotelId:guid}/add-ons")]
    public async Task<IActionResult> CreateAddOn(Guid hotelId, [FromBody] CreateHotelAddOnRequest request, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new CreateHotelAddOnCommand(hotelId, request.Code, request.Name, request.Description, request.PriceAmount, request.PriceCurrency, request.PricingType), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : StatusCode(201, result.Value);
    }

    [HttpPut("hotels/{hotelId:guid}/add-ons/{addOnId:guid}")]
    public async Task<IActionResult> UpdateAddOn(Guid hotelId, Guid addOnId, [FromBody] UpdateHotelAddOnRequest request, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new UpdateHotelAddOnCommand(hotelId, addOnId, request.Code, request.Name, request.Description, request.PriceAmount, request.PriceCurrency, request.PricingType), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("hotels/{hotelId:guid}/add-ons/{addOnId:guid}/{operation:regex(^(activate|deactivate)$)}")]
    public async Task<IActionResult> SetAddOnStatus(Guid hotelId, Guid addOnId, string operation, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return Forbid();
        var result = await accommodations.ExecuteCommandAsync(new SetHotelAddOnStatusCommand(hotelId, addOnId, operation == "activate"), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    private async Task<bool> CanAccessBooking(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await bookings.ExecuteQueryAsync(new GetByIdQuery(bookingId), cancellationToken);
        return booking is not null && await CanAccess(booking.HotelId, cancellationToken);
    }

    private async Task<bool> IsHotelRoom(Guid hotelId, Guid roomId, CancellationToken cancellationToken)
    {
        if (!await CanAccess(hotelId, cancellationToken)) return false;
        var rooms = await accommodations.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(hotelId, DateTime.UtcNow.Date), cancellationToken);
        return rooms.Any(x => x.RoomId == roomId);
    }

    private async Task<bool> CanAccess(Guid hotelId, CancellationToken cancellationToken)
    {
        if (User.IsInRole("Admin")) return true;
        if (!TryUser(out var userId)) return false;
        return await accommodations.ExecuteQueryAsync(new GetHotelOwnerQuery(hotelId), cancellationToken) == userId;
    }

    private bool TryUser(out Guid userId) => Guid.TryParse(User.FindFirstValue("sub"), out userId);
}

public sealed record AssignOwnerRequest(Guid? OwnerUserId);
public sealed record HotelierOverviewDto(int ArrivalsToday, int DeparturesToday, int ActiveStays, int NewBookings, IReadOnlyList<HotelBookingDto> NextActions);
public sealed record HotelierCalendarDto(object Rooms, IReadOnlyList<HotelBookingDto> Occupancy);
