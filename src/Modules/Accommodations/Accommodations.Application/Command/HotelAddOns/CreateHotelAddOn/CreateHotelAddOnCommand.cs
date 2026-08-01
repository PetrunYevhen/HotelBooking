using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.HotelAddOns.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.HotelAddOns.CreateHotelAddOn;

public sealed class CreateHotelAddOnCommand : CommandBase<Result<Guid>>
{
    public CreateHotelAddOnCommand(Guid hotelId, string code, string name, string? description, decimal priceAmount, string priceCurrency, PricingType pricingType)
    {
        HotelId = hotelId;
        Code = code;
        Name = name;
        Description = description;
        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;
        PricingType = pricingType;
    }
    public Guid HotelId { get; }
    public string Code { get; }
    public string Name { get; }
    public string? Description { get; }
    public decimal PriceAmount { get; }
    public string PriceCurrency { get; }
    public PricingType PricingType { get; }
}
