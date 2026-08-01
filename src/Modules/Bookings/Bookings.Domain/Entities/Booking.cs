using Bookings.Domain.Entities.Enums;
using Bookings.Domain.Entities.Events;
using Bookings.Domain.ValueObjects;
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
    public ICollection<BookingAddOn> AddOns { get; private set; } = new List<BookingAddOn>();
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CheckedInAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public BookingCompletionReason? CompletionReason { get; private set; }
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
        string? specialRequest,
        IReadOnlyCollection<BookingAddOnDetails>? addOns
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

        foreach (var addOn in addOns ?? [])
        {
            var addOnResult = BookingAddOn.Create(BookingId, addOn);
            if (addOnResult.IsSuccess)
                AddOns.Add(addOnResult.Value);
        }
    }

    public static Result<Booking> Create(
        Guid hotelId,
        Guid roomId,
        Guid userId,
        Money totalPrice,
        DateRange bookingDates,
        int guestsCount,
        GuestInfo guestInfo,
        string? specialRequest = null,
        IReadOnlyCollection<BookingAddOnDetails>? addOns = null)
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

        if (addOns?.Any(addOn => addOn.Quantity < 1 || addOn.UnitPrice.Currency != totalPrice.Currency || addOn.TotalPrice.Currency != totalPrice.Currency) == true)
            return Result.Failure<Booking>(new Error("Booking.InvalidAddOns", "Add-ons must have a positive quantity and use the booking currency."));

        return Result.Success(new Booking(hotelId, roomId, userId, totalPrice, bookingDates, guestsCount, guestInfo, specialRequest, addOns));
    }

    public Result Confirm()
    {
        if (Status != BookingStatus.Pending)
            return Result.Failure(new Error("Booking.InvalidState",
                $"Cannot confirm booking in status {Status}."));
        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        AddDomainEvent(new BookingConfirmedDomainEvent(BookingId.Value, RoomId, BookingDates, UserId, GuestInfo.Email));
        return Result.Success();
    }
    public Result Cancel(CancellationInitiator cancellationInitiator, Money refundAmount, string? cancellationReason = null)
    {
        if (Status is not (BookingStatus.Pending or BookingStatus.Confirmed))
            return Result.Failure(new Error("Booking.InvalidState",
                $"Cannot cancel booking in status {Status}."));

        Status = BookingStatus.Cancelled;
        CanceledAt = DateTime.UtcNow;
        CancelledBy = cancellationInitiator;
        CancellationReason = cancellationReason;
        AddDomainEvent(new BookingCanceledDomainEvent(BookingId, RoomId, cancellationInitiator, refundAmount));
        return Result.Success();
    }

    public Result CheckIn()
    {
        if(Status != BookingStatus.Confirmed)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot check in booking in status {Status}."));
        var today = DateTime.UtcNow.Date;
        if (today < BookingDates.Start.Date)
            return Result.Failure(new Error("Booking.EarlyCheckIn", "Check-in date has not arrived yet."));
        if (today > BookingDates.End.Date)
            return Result.Failure(new Error("Booking.LateCheckIn", "The booking checkout date has already passed."));

        Status = BookingStatus.CheckedIn;
        CheckedInAt = DateTime.UtcNow;
        AddDomainEvent(new BookingCheckedInDomainEvent(RoomId, BookingId));
        return Result.Success();
    }
    
    public Result CheckOutByStaff(DateTime utcNow)
    {
        return Complete(BookingCompletionReason.StaffCheckout, utcNow);
    }

    public Result CompleteAutomatically(DateTime utcNow)
    {
        if (Status != BookingStatus.CheckedIn)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot complete booking in status {Status}."));

        if (utcNow.Kind != DateTimeKind.Utc)
            return Result.Failure(new Error("Booking.InvalidUtc", "utcNow must be UTC."));

        if (utcNow < BookingDates.End)
            return Result.Failure(new Error(
                "Booking.CheckOutNotDue",
                "The booking checkout time has not arrived yet."));

        return Complete(BookingCompletionReason.AutomaticCheckout, utcNow);
    }

    private Result Complete(BookingCompletionReason reason, DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            return Result.Failure(new Error("Booking.InvalidUtc", "utcNow must be UTC."));

        if (Status != BookingStatus.CheckedIn)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot complete booking in status {Status}."));

        Status = BookingStatus.Completed;
        CompletedAt = utcNow;
        CompletionReason = reason;
        AddDomainEvent(new BookingCompletedDomainEvent(BookingId, HotelId, RoomId, GuestInfo, UserId, reason));
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

    public Result MarkNoShow()
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(new Error("Booking.InvalidState", $"Cannot mark no-show for booking in status {Status}."));

        Status = BookingStatus.NoShow;
        AddDomainEvent(new BookingNoShowDomainEvent(BookingId, RoomId));
        return Result.Success();
    }

    public bool IsRefundable(int refundWindowDays, DateTime utcNow)
    {
        if (refundWindowDays < 0)
            throw new ArgumentOutOfRangeException(nameof(refundWindowDays));
        if (utcNow.Kind != DateTimeKind.Utc)
            throw new ArgumentException("The current time must be UTC.", nameof(utcNow));

        return utcNow.Date <= BookingDates.Start.Date.AddDays(-refundWindowDays);
    }

    public Result<Money> CalculateRefundAmount(
        CancellationPolicyType policyType,
        int? deadlineDays,
        double? percentagePenalty,
        DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            return Result.Failure<Money>(new Error("Booking.InvalidUtc", "utcNow must be UTC."));

        if (policyType == CancellationPolicyType.NonRefundable)
            return Result.Success(Money.Zero(TotalPrice.Currency));

        var deadline = BookingDates.Start.Date.AddDays(-(deadlineDays ?? 0));
        var beforeDeadline = utcNow.Date <= deadline;

        if (beforeDeadline)
            return Result.Success(TotalPrice);

        if (policyType == CancellationPolicyType.FreeCancellation)
            return Result.Success(Money.Zero(TotalPrice.Currency));

        var refundFraction = (decimal)(100 - (percentagePenalty ?? 0)) / 100m;
        return TotalPrice.Multiply(refundFraction);
    }
}
