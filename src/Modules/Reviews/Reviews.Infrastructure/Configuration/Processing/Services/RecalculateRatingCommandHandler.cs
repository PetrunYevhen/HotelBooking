using BuildingBlock.Domain;
using MediatR;
using Reviews.Application.Services.RatingCalculator;
using Reviews.Domain.Entities.Snapshot;
using Reviews.Domain.RepositoryContract;

namespace Reviews.Infrastructure.Configuration.Processing.Services;

public class RecalculateRatingCommandHandler : IRequestHandler<RecalculateRatingCommand, Result>
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IHotelRatingSnapshotRepository _hotelRatingSnapshotRepository;
    private readonly IRatingCalculatorService _ratingCalculatorService;

    public RecalculateRatingCommandHandler(
        IReviewsRepository reviewsRepository,
        IHotelRatingSnapshotRepository hotelRatingSnapshotRepository,
        IRatingCalculatorService ratingCalculatorService)
    {
        _reviewsRepository = reviewsRepository;
        _hotelRatingSnapshotRepository = hotelRatingSnapshotRepository;
        _ratingCalculatorService = ratingCalculatorService;
    }

    public async Task<Result> Handle(RecalculateRatingCommand request, CancellationToken cancellationToken)
    {
        var hotelIds = await _reviewsRepository.GetHotelIdsWithPublishedReviewsAsync(cancellationToken);

        foreach (var hotelId in hotelIds)
        {
            var rating = await _ratingCalculatorService.CalculateForHotelAsync(hotelId, cancellationToken);

            var snapshot = HotelRatingSnapshot.Create(hotelId);
            snapshot.UpdateRating(rating);

            await _hotelRatingSnapshotRepository.AddAsync(snapshot, cancellationToken);
        }

        return Result.Success();
    }
}
