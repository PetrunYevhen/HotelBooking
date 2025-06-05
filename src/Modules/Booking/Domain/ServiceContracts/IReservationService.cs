using Booking.Domain.Entities;

namespace Booking.Domain.ServiceContracts;

public interface IReservationService
{
    Task<Reservation> GetReservationByIdAsync(ReservationId reservationId);
    Task<List<Reservation>> GetAllReservationsAsync();
    Task<Reservation> CreateAsync(Reservation reservation);
}