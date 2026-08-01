using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities;

public sealed class HotelAddOnSnapshot : Entity
{
    public Guid HotelAddOnId { get; private set; }
    public Guid HotelId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Money Price { get; private set; } = null!;
    public HotelAddOnPricingType PricingType { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private HotelAddOnSnapshot() { }

    private HotelAddOnSnapshot(Guid hotelAddOnId, Guid hotelId, string code, string name, string? description,
        Money price, HotelAddOnPricingType pricingType, bool isActive)
    {
        HotelAddOnId = hotelAddOnId;
        HotelId = hotelId;
        Code = code;
        Name = name;
        Description = description;
        Price = price;
        PricingType = pricingType;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<HotelAddOnSnapshot> Create(Guid hotelAddOnId, Guid hotelId, string code, string name,
        string? description, Money price, HotelAddOnPricingType pricingType, bool isActive)
    {
        if (hotelAddOnId == Guid.Empty || hotelId == Guid.Empty)
            return Result.Failure<HotelAddOnSnapshot>(new Error("HotelAddOnSnapshot.InvalidId", "Hotel and add-on identifiers are required."));
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name) || price is null || !Enum.IsDefined(pricingType))
            return Result.Failure<HotelAddOnSnapshot>(new Error("HotelAddOnSnapshot.InvalidData", "Add-on snapshot data is invalid."));
        return Result.Success(new HotelAddOnSnapshot(hotelAddOnId, hotelId, code.Trim().ToLowerInvariant(), name.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(), price, pricingType, isActive));
    }

    public Result Update(string code, string name, string? description, Money price, HotelAddOnPricingType pricingType, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name) || price is null || !Enum.IsDefined(pricingType))
            return Result.Failure(new Error("HotelAddOnSnapshot.InvalidData", "Add-on snapshot data is invalid."));

        Code = code.Trim().ToLowerInvariant();
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Price = price;
        PricingType = pricingType;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
