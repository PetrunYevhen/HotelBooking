using BookingManagement.Domain.Enums;

namespace BookingManagement.Domain.Entities;

public class Booking
{
    public BookingId BookingId { get; set; }
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingStatus Status { get; set; }

    private Booking() {} // EF Domain requires a parameterless constructor for entity instantiation
    
    public Booking(
        BookingId bookingId,
        Guid guestId,
        Guid roomId,
        decimal price,
        DateTime startDate,
        DateTime endDate,
        BookingStatus status
)    {
        if (startDate > endDate) 
            throw new InvalidOperationException("Start date cannot be greater than end date");
        
        if(price < 0) 
            throw new InvalidOperationException("Price cannot be negative");
        
        BookingId = bookingId;
        GuestId = guestId;
        RoomId = roomId;
        Price = price;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
    
    public void Approve()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending reservations can be approved.");
        
        Status = BookingStatus.Confirmed;
    }

    public void Canceled()
    {
        if(Status != BookingStatus.Canceled)
            throw new InvalidOperationException("Only pending reservations can be canceled.");
        
        Status = BookingStatus.Canceled;
    }

    public void ChangeDates(DateTime newStartDate, DateTime newEndDate)
    {
        if (Status != BookingStatus.Pending)
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