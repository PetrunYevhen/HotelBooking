using Booking.Domain.Entities;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Query;

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<Reservation>>
{
    private readonly IReservationReadRepository _reservationReadRepository;
    
    public GetAllReservationsQueryHandler(IReservationReadRepository reservationReadRepository)
    {
        _reservationReadRepository = reservationReadRepository;
    }

    public async Task<List<Reservation>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        return await _reservationReadRepository.GetAllReservationsAsync(cancellationToken);
    }
}