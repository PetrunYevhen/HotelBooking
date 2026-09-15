using SharedKernel.Contracts;
using BuildingBlock.Domain;
using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.HotelierApplications;

namespace Accommodations.Application.HotelierApplications;

public sealed class ReviewHotelierApplicationCommand(HotelierApplicationId applicationId, AccountId reviewerId, bool approve, string? rejectionReason) : CommandBase<Result>
{
    public HotelierApplicationId ApplicationId { get; } = applicationId;
    public AccountId ReviewerId { get; } = reviewerId;
    public bool Approve { get; } = approve;
    public string? RejectionReason { get; } = rejectionReason;
    public bool IsAdmin { get; init; }
}
