using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Command;

public class AddReservationCommandHandler : IRequestHandler<AddReservationCommand, Reservation>
{
    private readonly IReservationRepository _reservationRepository;
    
    public AddReservationCommandHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }
    
    public async Task<Reservation> Handle(AddReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = new Reservation
        (
            new ReservationId(Guid.NewGuid()),
            request.GuestId,
            request.RoomId,
            request.Price,
            request.StartDate,
            request.EndDate,
            ReservationStatus.Pending
        );
        await _reservationRepository.AddReservationAsync(reservation, cancellationToken);
        return reservation;

    }
}