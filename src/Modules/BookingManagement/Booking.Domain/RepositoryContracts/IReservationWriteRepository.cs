namespace BookingManagement.Domain.RepositoryContracts;

public interface IReservationWriteRepository 
{
    Task AddReservationAsync(Entities.Booking booking, CancellationToken cancellationToken);
    Task UpdateReservationAsync(Entities.Booking booking, CancellationToken cancellationToken);
}