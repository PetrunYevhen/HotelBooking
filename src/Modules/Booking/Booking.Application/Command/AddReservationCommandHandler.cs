using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.RepositoryContracts;
using MediatR;

namespace Application.Command;

public class AddReservationCommandHandler : IRequestHandler<AddReservationCommand, Reservation>
{
    private readonly IReservationWriteRepository _reservationWriteRepository;
    
    public AddReservationCommandHandler(IReservationWriteRepository reservationWriteRepository)
    {
        _reservationWriteRepository = reservationWriteRepository;
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
        
        await _reservationWriteRepository.AddReservationAsync(reservation, cancellationToken);
        return reservation;

    }
}