using Bookings.Application.Contracts;

namespace Bookings.Application.Query.GetBookingsByHotelId;

public sealed class GetBookingsByHotelIdQuery(Guid hotelId, DateTime? from = null, DateTime? to = null, string? status = null, Guid? roomId = null)
    : QueryBase<List<HotelBookingDto>>
{
    public Guid HotelId { get; } = hotelId;
    public DateTime? From { get; } = from;
    public DateTime? To { get; } = to;
    public string? Status { get; } = status;
    public Guid? RoomId { get; } = roomId;
}
