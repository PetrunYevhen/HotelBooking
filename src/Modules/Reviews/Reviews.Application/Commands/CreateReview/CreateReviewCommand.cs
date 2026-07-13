using BuildingBlock.Domain;
using Reviews.Application.Contracts;

namespace Reviews.Application.Commands.CreateReview;

public class CreateReviewCommand : CommandBase<Result>
{
    public Guid HotelId { get; init; }
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public double Rating { get; init; }
    public string Comment { get; init; }
}