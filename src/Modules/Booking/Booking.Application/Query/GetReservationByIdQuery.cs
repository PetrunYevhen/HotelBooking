using Booking.Domain.Entities;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Query;

public class GetReservationByIdQuery : IRequest<Reservation>
{
    public ReservationId ReservationId { get; }
    
    public GetReservationByIdQuery(ReservationId reservationId)
    {
        ReservationId = reservationId;
    }
    
    
}