using SharedKernel.ValueObjects;

namespace Accommodations.Application.Services.Pricing;

public interface IPriceCalculationService
{
    Money Calculate(Money basePrice, int demandScore, DateTime date);

}