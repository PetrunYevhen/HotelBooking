using BookingManagement.Application.Contracts;
using BookingManagement.Domain.Enums;

namespace BookingManagement.Application.Command.CreateBooking;

public class CreateBookingCommand : CommandBase<Guid>
{
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public int UserId { get; init; } = 1;
    
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }

    public CreateBookingCommand(Guid hotelId, Guid roomId, int userId, DateTime checkInDate, DateTime checkOutDate)
    {
        HotelId = hotelId;
        RoomId = roomId;
        UserId = userId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        
    }
}