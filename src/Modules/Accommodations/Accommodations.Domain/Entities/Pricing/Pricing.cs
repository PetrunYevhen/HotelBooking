using Accommodations.Domain.Entities.Pricing.Enums;
using Accommodations.Domain.Entities.Rooms;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Pricing;

public class Pricing : Entity
{
    public PricingId PricingId { get; private set; }
    public RoomId RoomId { get; private set; }
    public Money Price { get; private set; }
    public PricingType Type { get; private set; }
    public DateRange ValidDates { get; private set; }
    public bool IsActive { get; private set; }
    
    private Pricing(){}

    private Pricing(
        RoomId roomId,
        Money price,
        PricingType type,
        DateRange validDates,
        bool isActive
    )
    {
        PricingId = PricingId.New();
        RoomId = roomId;
        Price = price;
        Type = type;
        ValidDates = validDates;
        IsActive = isActive;
    }

    public static Result<Pricing> Create(RoomId roomId, Money price, PricingType type, DateRange validDates
        )
    {
        if (roomId is null) 
            return Result.Failure<Pricing>(new Error("Pricing.InvalidRoomId", "RoomId is null"));
        if (price is null) 
            return Result.Failure<Pricing>(new Error("Pricing.InvalidPrice", "Price is null"));
        if (validDates is null) 
            return Result.Failure<Pricing>(new Error("Pricing.InvalidDates", "Dates are null"));
        return Result.Success(new Pricing(roomId, price, type, validDates, isActive: true));
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
}
