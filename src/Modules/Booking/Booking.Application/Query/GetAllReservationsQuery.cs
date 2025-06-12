using Booking.Domain.Entities;
using MediatR;

namespace Application.Query;

public class GetAllReservationsQuery : IRequest<List<Reservation>>
{
    
}