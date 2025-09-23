using BookingManagement.Application.Contracts;
using BookingManagement.Domain.Entities;

namespace BookingManagement.Application.Command;

public class AddBookingCommand : CommandBase<Booking>
{
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public AddBookingCommand(Guid guestId, Guid roomId, decimal price, DateTime startDate, DateTime endDate)
    {
        GuestId = guestId;
        RoomId = roomId;
        Price = price;
        StartDate = startDate;
        EndDate = endDate;
    }
}