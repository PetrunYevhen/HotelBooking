using BuildingBlock.Domain;
using MediatR;
using Accommodations.Domain.RepositoryContract.HotelierApplications;

namespace Accommodations.Application.HotelierApplications;

public sealed class ReviewHotelierApplicationCommandHandler(IHotelierApplicationRepository applications) : IRequestHandler<ReviewHotelierApplicationCommand, Result>
{
    public async Task<Result> Handle(ReviewHotelierApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applications.GetByIdAsync(request.ApplicationId, cancellationToken);
        if (application is null) return Result.Failure(new Error("HotelierApplication.NotFound", "Application not found."));
        if (!request.IsAdmin || request.ReviewerId.Value == Guid.Empty)
            return Result.Failure(new Error("User.NotAdmin", "Only administrators can review applications."));

        var result = request.Approve
            ? application.Approve(request.ReviewerId)
            : application.Reject(request.ReviewerId, request.RejectionReason ?? string.Empty);
        if (result.IsFailure) return result;
        await applications.UpdateAsync(application, cancellationToken);
        return Result.Success();
    }
}
