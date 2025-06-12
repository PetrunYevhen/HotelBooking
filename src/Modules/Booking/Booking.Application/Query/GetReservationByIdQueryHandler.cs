using Booking.Domain.Entities;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Query;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Reservation>
{
    private readonly IReservationReadRepository _reservationReadRepository;
    
    public GetReservationByIdQueryHandler(IReservationReadRepository reservationReadRepository)
    {
        _reservationReadRepository = reservationReadRepository;
    }
    
    public Task<Reservation> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        return _reservationReadRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);
    }
}