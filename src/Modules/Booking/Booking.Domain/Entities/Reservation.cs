using Booking.Domain.Enums;

namespace Booking.Domain.Entities;

public class Reservation
{
    public ReservationId ReservationId { get; set; }
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus Status { get; set; }

    private Reservation()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }
    
    public Reservation(
        ReservationId reservationId,
        Guid guestId,
        Guid roomId,
        decimal price,
        DateTime startDate,
        DateTime endDate,
        ReservationStatus status)
    {
        if (startDate > endDate) 
            throw new InvalidOperationException("Start date cannot be greater than end date");
        
        ReservationId = reservationId;
        GuestId = guestId;
        RoomId = roomId;
        Price = price;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
    
    public void Approve()
    {
        if (Status != ReservationStatus.Pending)
            throw new InvalidOperationException("Only pending reservations can be approved.");
        
        Status = ReservationStatus.Confirmed;
    }

    public void Canceled()
    {
        if(Status != ReservationStatus.Canceled)
            throw new InvalidOperationException("Only pending reservations can be canceled.");
        
        Status = ReservationStatus.Canceled;
    }

    public void ChangeDates(DateTime newStartDate, DateTime newEndDate)
    {
        if (Status != ReservationStatus.Pending)
            throw new InvalidOperationException("Only pending reservations can be changed.");
        
        if(newStartDate > newEndDate) 
            throw new InvalidOperationException("Start date cannot be greater than end date");
        
        StartDate = newStartDate;
        EndDate = newEndDate;
    }
    
    public decimal CalculateTotalPrice(decimal dailyRate)
    {
        if (dailyRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(dailyRate), "Daily rate must be greater than zero.");
        
        if (StartDate >= EndDate)
            throw new InvalidOperationException("Start date must be before end date to calculate total price.");
        
        var totalDays = (EndDate - StartDate).Days;
        return totalDays * dailyRate;
    }
    
}