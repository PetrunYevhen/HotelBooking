using Infrastructure.EventBus;

namespace Accommodations.IntegrationEvents;

public sealed class HotelAddOnDeactivatedIntegrationEvent : IntegrationEvent
{
    public HotelAddOnDeactivatedIntegrationEvent(Guid id, DateTime occurredOn, Guid hotelAddOnId, Guid hotelId,
        string code, string name, string? description, decimal priceAmount, string priceCurrency, int pricingType)
        : base(id, occurredOn)
    {
        HotelAddOnId = hotelAddOnId;
        HotelId = hotelId;
        Code = code;
        Name = name;
        Description = description;
        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;
        PricingType = pricingType;
    }

    public Guid HotelAddOnId { get; }
    public Guid HotelId { get; }
    public string Code { get; }
    public string Name { get; }
    public string? Description { get; }
    public decimal PriceAmount { get; }
    public string PriceCurrency { get; }
    public int PricingType { get; }
}
