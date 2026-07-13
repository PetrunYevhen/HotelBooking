using SharedKernel.ValueObjects;

namespace Accommodations.Application.Services.Pricing;

public interface IPriceCalculationService
{
    // Task<Result<Money>> CalculateAsync(Guid roomId, DateTime start, DateTime end, CancellationToken ct);
    Money Calculate(Money basePrice, int demandScore, DateTime date);

}