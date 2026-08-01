using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.HotelAddOns.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.HotelAddOns.UpdateHotelAddOn;

public sealed class UpdateHotelAddOnCommand : CommandBase<Result>
{
    public UpdateHotelAddOnCommand(Guid hotelId, Guid hotelAddOnId, string code, string name, string? description, decimal priceAmount, string priceCurrency, PricingType pricingType)
    {
        HotelId = hotelId;
        HotelAddOnId = hotelAddOnId;
        Code = code;
        Name = name;
        Description = description;
        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;
        PricingType = pricingType;
    }
    public Guid HotelId { get; }
    public Guid HotelAddOnId { get; }
    public string Code { get; }
    public string Name { get; }
    public string? Description { get; }
    public decimal PriceAmount { get; }
    public string PriceCurrency { get; }
    public PricingType PricingType { get; }
}
