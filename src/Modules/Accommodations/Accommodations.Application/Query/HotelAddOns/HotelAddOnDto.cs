namespace Accommodations.Application.Query.HotelAddOns;

public sealed class HotelAddOnDto
{
    public Guid HotelAddOnId { get; init; }
    public Guid HotelId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; } = string.Empty;
    public int PricingType { get; init; }
    public bool IsActive { get; init; }
}
