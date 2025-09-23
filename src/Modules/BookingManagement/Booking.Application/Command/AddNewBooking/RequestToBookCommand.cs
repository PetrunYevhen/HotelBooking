using BookingManagement.Domain.Entities;
using MediatR;

namespace BookingManagement.Application.Command.AddNewBooking;

public class RequestToBookCommand : IRequest<Booking>
{
    public string Title = "Request to Book";
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int GuestCount { get; set; }
    
    
    // Payment details
    // public PaymentMethodDto PaymentMethod { get; set; }
    // public PaymentDetailsDto PaymentDetails { get; set; }
    //
    // public decimal TotalPayment => PaymentDetails?.TotalAmount ?? 0;
    
}