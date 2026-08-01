using Accommodations.Domain.Entities.HotelAddOns.Enums;
using Accommodations.Domain.Entities.HotelAddOns.Events;
using Accommodations.Domain.Entities.Hotels;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.HotelAddOns;

public sealed class HotelAddOn : Entity, IAggregateRoot
{
    public HotelAddOnId HotelAddOnId { get; private set; } = null!;
    public HotelId HotelId { get; private set; } = null!;
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Money Price { get; private set; } = null!;
    public PricingType PricingType { get; private set; }
    public bool IsActive { get; private set; }

    private HotelAddOn() { }

    private HotelAddOn(HotelId hotelId, string code, string name, string? description, Money price, PricingType pricingType)
    {
        HotelAddOnId = HotelAddOnId.New();
        HotelId = hotelId;
        Code = code;
        Name = name;
        Description = description;
        Price = price;
        PricingType = pricingType;
        IsActive = true;
        RaiseUpserted();
    }

    public static Result<HotelAddOn> Create(
        HotelId hotelId,
        string code,
        string name,
        string? description,
        Money price,
        PricingType pricingType)
    {
        var validation = Validate(hotelId, code, name, price, pricingType);
        return validation.IsFailure
            ? Result.Failure<HotelAddOn>(validation.Error)
            : Result.Success(new HotelAddOn(hotelId, NormalizeCode(code), name.Trim(), NormalizeDescription(description), price, pricingType));
    }

    public Result Update(string code, string name, string? description, Money price, PricingType pricingType)
    {
        var validation = Validate(HotelId, code, name, price, pricingType);
        if (validation.IsFailure)
            return validation;

        Code = NormalizeCode(code);
        Name = name.Trim();
        Description = NormalizeDescription(description);
        Price = price;
        PricingType = pricingType;
        RaiseUpserted();
        return Result.Success();
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        RaiseUpserted();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        AddDomainEvent(new HotelAddOnDeactivatedDomainEvent(HotelAddOnId, HotelId.Value, Code, Name, Description, Price, PricingType));
    }

    private void RaiseUpserted() => AddDomainEvent(new HotelAddOnUpsertedDomainEvent(
        HotelAddOnId, HotelId.Value, Code, Name, Description, Price, PricingType, IsActive));

    private static Result Validate(HotelId hotelId, string code, string name, Money price, PricingType pricingType)
    {
        if (hotelId is null || hotelId.Value == Guid.Empty)
            return Result.Failure(new Error("HotelAddOn.InvalidHotelId", "HotelId is required."));
        if (string.IsNullOrWhiteSpace(code) || code.Trim().Length > 50)
            return Result.Failure(new Error("HotelAddOn.InvalidCode", "Code is required and must be at most 50 characters."));
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 120)
            return Result.Failure(new Error("HotelAddOn.InvalidName", "Name is required and must be at most 120 characters."));
        if (price is null)
            return Result.Failure(new Error("HotelAddOn.InvalidPrice", "Price is required."));
        if (!Enum.IsDefined(pricingType))
            return Result.Failure(new Error("HotelAddOn.InvalidPricingType", "Pricing type is invalid."));
        return Result.Success();
    }

    private static string NormalizeCode(string code) => code.Trim().ToLowerInvariant();
    private static string? NormalizeDescription(string? description) => string.IsNullOrWhiteSpace(description) ? null : description.Trim();
}
