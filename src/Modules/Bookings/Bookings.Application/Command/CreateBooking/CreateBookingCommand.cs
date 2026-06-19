using Bookings.Application.Contracts;
using BuildingBlock.Domain;

namespace Bookings.Application.Command.CreateBooking;

public class CreateBookingCommand : CommandBase<Result<Guid>>
{
    public CreateBookingCommand(Guid hotelId, Guid roomId, Guid userId, DateTime checkIn, DateTime checkOut, int guestCount, string firstName, string lastName, string email, string phoneNumber, string? specialRequest)
    {
        HotelId = hotelId;
        RoomId = roomId;
        UserId = userId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        GuestCount = guestCount;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        SpecialRequest = specialRequest;
    }

    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public Guid UserId { get; init; }
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
    public int GuestCount { get; init; }
    
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }

    public string? SpecialRequest { get; init; }
}