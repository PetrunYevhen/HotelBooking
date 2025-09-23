using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using Infrastructure.EventBus;
using RoomManagment.IntegrationEvent.Booking;

namespace BookingManagement.Application.EventHandler
{
    public class GetPricePerNightResponseIntegrationEventHandler 
        : IIntegrationEventHandler<GetPricePerNightResponseIntegrationEvent>
    {
        private readonly IReservationWriteRepository _reservationWriteRepository;
        private readonly IReservationReadRepository _reservationReadRepository;

        public GetPricePerNightResponseIntegrationEventHandler(IReservationWriteRepository reservationWriteRepository, IReservationReadRepository reservationReadRepository)
        {
            _reservationWriteRepository = reservationWriteRepository;
            _reservationReadRepository = reservationReadRepository;
        }

        public async Task Handle(GetPricePerNightResponseIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            var bookingId = new BookingId(@event.BookingId);
            var booking = await _reservationReadRepository.GetReservationByIdAsync(bookingId, cancellationToken);
            if (booking == null)
                throw new System.Exception($"Booking {@event.BookingId} not found");

            var totalPrice = booking.CalculateTotalPrice(@event.PricePerNight);

            booking.Price = totalPrice;

            booking.Approve();

            await _reservationWriteRepository.UpdateReservationAsync(booking, cancellationToken);
        }
    }
}