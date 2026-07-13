namespace Reviews.Application.Services.RatingCalculator;

public interface IRatingCalculatorService
{
    Task<double> CalculateForHotelAsync(Guid hotelId, CancellationToken cancellationToken);
}