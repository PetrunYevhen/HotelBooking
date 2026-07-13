using Reviews.Domain.RepositoryContract;

namespace Reviews.Application.Services.RatingCalculator;

public class RatingCalculatorService : IRatingCalculatorService
{
    private const double DecayLambda = 0.005;
    private const double VerifiedMultiplier = 1.2;
    private const double BayesianC = 10.0;
    private const double GlobalMean = 3.5;
    
    private readonly IReviewsRepository  _reviewsRepository;

    public RatingCalculatorService(IReviewsRepository reviewsRepository)
    {
        _reviewsRepository = reviewsRepository;
    }

    public async Task<double> CalculateForHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var reviews = await _reviewsRepository.GetByHotelIdAsync(hotelId, cancellationToken);

        var today = DateTime.UtcNow;
        double weightedSum = 0;
        double totalWeight = 0;

        foreach (var review in reviews)
        {
            var daySince = (today - review.CreatedAt).TotalDays;
            var recency = Math.Exp(-DecayLambda * daySince);
            var verified = review.IsBookingVerified ? VerifiedMultiplier : 1.0;
            
            var weight = recency * verified;
            
            weightedSum += review.Rating.Value * weight;
            totalWeight += weight;
        }
        
        var rawScore = weightedSum / totalWeight;
        var bayesian = (BayesianC * GlobalMean + rawScore * totalWeight) / (BayesianC + totalWeight);
        
        return Math.Round(bayesian, 2);
    }
}