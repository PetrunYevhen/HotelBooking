using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using MediatR;

namespace BookingManagement.Application.Query.GetBookingDetails;

public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, Booking>
{
    private readonly IReservationReadRepository _reservationReadRepository;
    
    public GetBookingDetailsQueryHandler(IReservationReadRepository reservationReadRepository)
    {
        _reservationReadRepository = reservationReadRepository;
    }
    
    public Task<Booking> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        return _reservationReadRepository.GetReservationByIdAsync(request.BookingId, cancellationToken);
    }
}