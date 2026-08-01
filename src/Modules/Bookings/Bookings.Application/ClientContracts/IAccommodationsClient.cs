using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.ClientContracts;

public interface IAccommodationsClient
{
    Task<bool> IsRoomAvailableAsync(Guid roomId,  CancellationToken cancellationToken);
    Task<Result<Money>> GetRoomPriceAsync(Guid roomId, DateRange dateRange, CancellationToken cancellationToken);
    Task<int> GetHotelCheckOutHoursAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<CancellationPolicyDto> GetHotelCancellationPolicyAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<HotelAddOnConfigurationDto?> GetHotelAddOnAsync(Guid hotelId, Guid hotelAddOnId, CancellationToken cancellationToken);
}

public sealed class HotelAddOnConfigurationDto
{
    public Guid HotelAddOnId { get; set; }
    public Guid HotelId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = string.Empty;
    public int PricingType { get; set; }
    public bool IsActive { get; set; }
}

public class CancellationPolicyDto
{
    public int Type { get; set; }
    public int? DeadlineDays { get; set; }
    public double? PercentagePenalty { get; set; }
}
