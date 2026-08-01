using Accommodations.Domain.Entities.HotelAddOns.Enums;
using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.HotelAddOns.Events;

public sealed class HotelAddOnUpsertedDomainEvent : DomainEventBase
{
    public HotelAddOnUpsertedDomainEvent(
        HotelAddOnId hotelAddOnId,
        Guid hotelId,
        string code,
        string name,
        string? description,
        Money price,
        PricingType pricingType,
        bool isActive)
    {
        HotelAddOnId = hotelAddOnId;
        HotelId = hotelId;
        Code = code;
        Name = name;
        Description = description;
        Price = price;
        PricingType = pricingType;
        IsActive = isActive;
    }

    public HotelAddOnId HotelAddOnId { get; }
    public Guid HotelId { get; }
    public string Code { get; }
    public string Name { get; }
    public string? Description { get; }
    public Money Price { get; }
    public PricingType PricingType { get; }
    public bool IsActive { get; }
}
