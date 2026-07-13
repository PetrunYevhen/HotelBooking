using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Pricing.SetRoomPricing;

public class SetRoomPricingCommand : CommandBase<Result>
{
    public SetRoomPricingCommand(Guid roomId, decimal price, string currency, DateTime validFrom, DateTime validTo)
    {
        RoomId = roomId;
        Price = price;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public Guid RoomId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}