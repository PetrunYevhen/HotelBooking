using BuildingBlock.Domain;
using MediatR;
using Reviews.Domain.Entities.Reviews;
using Reviews.Domain.RepositoryContract;
using Reviews.Domain.ValueObjects;

namespace Reviews.Application.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result>
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IEligibleReviewRepository _eligibleReviewRepository;

    public CreateReviewCommandHandler(IReviewsRepository reviewsRepository, IEligibleReviewRepository eligibleReviewRepository)
    {
        _reviewsRepository = reviewsRepository;
        _eligibleReviewRepository = eligibleReviewRepository;
    }

    public async Task<Result> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var eligible = await _eligibleReviewRepository
            .GetByBookingIdAsync(request.BookingId, cancellationToken);
        if (eligible is null)
            return Result.Failure(new Error("Review.NotEligible", "Booking is not completed."));
       
        if(eligible.UserId != request.UserId)
            return Result.Failure(new Error("Review.Unauthorized", "Booking does not belong to user."));

        var ratingResult = RatingScore.Create(request.Rating);
        if (ratingResult.IsFailure)
            return Result.Failure(ratingResult.Error);
        
        var reviewResult = Review.Create(
            request.BookingId,
            request.HotelId,
            request.UserId,
            ratingResult.Value,
            request.Comment);

        if (reviewResult.IsFailure)
            return Result.Failure(reviewResult.Error);
        
        reviewResult.Value.Verify(request.BookingId);

        var publishResult = reviewResult.Value.Publish();
        if (publishResult.IsFailure)
            return publishResult;

        await _reviewsRepository.AddAsync(reviewResult.Value, cancellationToken);
        
        return Result.Success();
    }
}