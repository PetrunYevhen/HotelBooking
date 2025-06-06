using Booking.Domain.Entities;
using MediatR;

namespace Application.Command;

public class AddReservationCommand : IRequest<Reservation>
{
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}