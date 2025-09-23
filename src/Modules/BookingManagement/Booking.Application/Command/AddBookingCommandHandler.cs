using BookingManagement.Application.Exception;
using BookingManagement.Domain.Entities;
using BookingManagement.Domain.Enums;
using BookingManagement.Domain.RepositoryContracts;
using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace BookingManagement.Application.Command;

public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, Booking>
{
    private readonly IReservationWriteRepository _reservationWriteRepository;
    private readonly IReservationReadRepository _reservationReadRepository;
    private readonly IEventBus _eventBus;

    public AddBookingCommandHandler(IReservationWriteRepository reservationWriteRepository, IReservationReadRepository reservationReadRepository, IEventBus eventBus)
    {
        _reservationWriteRepository = reservationWriteRepository;
        _reservationReadRepository = reservationReadRepository;
        _eventBus = eventBus;
    }

    public async Task<Booking> Handle(AddBookingCommand request,
        CancellationToken cancellationToken)
    {
        var checkRoomAvailability = await _reservationReadRepository.CheckRoomAvailabilityAsync(request.RoomId, request.StartDate, request.EndDate, cancellationToken);
        if (!checkRoomAvailability)
        {
            throw new RoomNotAvailableException(request.RoomId, request.StartDate, request.EndDate);
        }
        
        var bookingId = new BookingId(Guid.NewGuid());

        var booking = new Booking(
            bookingId,
            request.GuestId,
            request.RoomId,
            request.Price,
            request.StartDate,
            request.EndDate,
            BookingStatus.Pending
        );

        
        await _reservationWriteRepository.AddReservationAsync(booking, cancellationToken);

        var priceRequestEvent = new GetPricePerNightRequestIntegrationEvent(
            Guid.NewGuid(),
            request.RoomId,
            DateTime.UtcNow,
            bookingId.Value
        );
        await _eventBus.Publish(priceRequestEvent, cancellationToken); 
        
        return booking;
    }
}