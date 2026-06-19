using Bookings.Domain.Entities.Enums;
using Bookings.Domain.Entities.Events;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities;

public class Booking : Entity, IAggregateRoot
{
    public BookingId BookingId { get; private set; }
    public Guid HotelId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid UserId { get; private set; }
    
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateRange BookingDates { get; private set; }
    public int GuestsCount { get; private set; }
    public GuestInfo GuestInfo { get; private set; }
    public string? SpecialRequest { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    
    public CancellationInitiator? CancelledBy { get; private set; } 
    public string? CancellationReason { get; private set; }

    private Booking() {} 
    
    private Booking(
        Guid hotelId,
        Guid roomId,
        Guid userId,
        Money totalPrice,
        DateRange bookingDates,
        int guestsCount,
        GuestInfo guestInfo,
        string? specialRequest
)
    {
        BookingId = BookingId.New();
        HotelId = hotelId;
        RoomId = roomId;
        UserId = userId;
        TotalPrice = totalPrice;
        BookingDates = bookingDates;
        GuestsCount = guestsCount;
        GuestInfo = guestInfo;
        SpecialRequest = specialRequest;
        Status = BookingStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new BookingCreatedDomainEvent(
            BookingId, HotelId, RoomId,
            BookingDates, TotalPrice));
    }

    public static Result<Booking> Create(
        Guid hotelId,
        Guid roomId,
        Guid userId,
        Money totalPrice,
        DateRange bookingDates,
        int guestsCount,
        GuestInfo guestInfo,
        string? specialRequest = null)
    {
        if (hotelId == Guid.Empty)
            return Result.Failure<Booking>(new Error("Booking.InvalidHotelId", "HotelId is required."));
        if (roomId == Guid.Empty)
            return Result.Failure<Booking>(new Error("Booking.InvalidRoomId", "RoomId is required."));
        if (userId == Guid.Empty)
            return Result.Failure<Booking>(new Error("Booking.InvalidUserId", "UserId is required."));
        if (totalPrice is null)
            return Result.Failure<Booking>(new Error("Booking.InvalidPrice", "TotalPrice is required."));
        if (bookingDates is null)
            return Result.Failure<Booking>(new Error("Booking.InvalidDates", "BookingDates is required."));
        if (guestsCount <= 0)
            return Result.Failure<Booking>(new Error("Booking.InvalidGuestsCount", "GuestsCount must be greater than zero."));
        if (guestInfo is null)
            return Result.Failure<Booking>(new Error("Booking.InvalidGuestInfo", "GuestInfo is required."));

        return Result.Success(new Booking(hotelId, roomId, userId, totalPrice,  bookingDates, guestsCount, guestInfo, specialRequest));
    }

    public Result Confirm()
    {
        if (Status != BookingStatus.Pending)
            return Result.Failure(new Error("Booking.InvalidState",
                $"Cannot confirm booking in status {Status}."));
        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        AddDomainEvent(new BookingConfirmedDomainEvent(HotelId, RoomId, BookingDates));
        return Result.Success();
    }
    public Result Cancel(CancellationInitiator cancellationInitiator, string? cancellationReason = null)
    {
        if(Status is BookingStatus.Cancelled or BookingStatus.Completed)
            return Result.Failure(new Error("Booking.InvalidState",
                $"Cannot cancel booking in status {Status}."));

        Status = BookingStatus.Cancelled;
        CanceledAt = DateTime.UtcNow;
        CancelledBy = cancellationInitiator;
        CancellationReason = cancellationReason;
        AddDomainEvent(new BookingCanceledDomainEvent(BookingId, RoomId, cancellationInitiator));
        return Result.Success();
    }

    public Result CheckIn()
    {
        if(Status != BookingStatus.Confirmed)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot check in booking in status {Status}."));
        if (DateTime.UtcNow.Date < BookingDates.Start.Date)
            return Result.Failure(new Error("Booking.EarlyCheckIn", "Check-in date has not arrived yet."));

        Status = BookingStatus.CheckedIn;
        AddDomainEvent(new BookingCheckedInDomainEvent(RoomId, BookingId));
        return Result.Success();
    }
    
    public Result CheckOut()
    {
        if (Status != BookingStatus.CheckedIn)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot check out booking in status {Status}."));

        Status = BookingStatus.Completed;
        AddDomainEvent(new BookingCompletedDomainEvent(BookingId, HotelId, RoomId, GuestInfo));
        return Result.Success();
    }
    
    public Result Expire()
    {
        if (Status != BookingStatus.Pending)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot expire booking in status {Status}."));

        Status = BookingStatus.Cancelled;
        CanceledAt = DateTime.UtcNow;
        CancelledBy = CancellationInitiator.System;
        CancellationReason = "Booking expired — payment not confirmed in time.";
        AddDomainEvent(new BookingExpiredDomainEvent(BookingId, RoomId));
        return Result.Success();
    }

    public bool IsRefundable(int refundWindowDay)
    {
        return DateTime.Now.Date >= BookingDates.Start.Date.AddDays(-refundWindowDay);
    }
}