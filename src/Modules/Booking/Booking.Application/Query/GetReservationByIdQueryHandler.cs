using Booking.Domain.Entities;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Query;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Reservation>
{
    private readonly IReservationRepository _reservationRepository;
    
    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }
    
    public Task<Reservation> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        return _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);
    }
}