using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using MediatR;

namespace BookingManagement.Application.Query;

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<Booking>>
{
    private readonly IReservationReadRepository _reservationReadRepository;
    
    public GetAllReservationsQueryHandler(IReservationReadRepository reservationReadRepository)
    {
        _reservationReadRepository = reservationReadRepository;
    }

    public async Task<List<Booking>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        return await _reservationReadRepository.GetAllReservationsAsync(cancellationToken);
    }
}