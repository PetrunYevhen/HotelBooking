using BuildingBlock.Domain;
using MediatR;
using Accommodations.Domain.Entities.HotelierApplications;
using Accommodations.Domain.RepositoryContract.HotelierApplications;

namespace Accommodations.Application.HotelierApplications;

public sealed class SubmitHotelierApplicationCommandHandler(IHotelierApplicationRepository hotelierApplicationRepository) : IRequestHandler<SubmitHotelierApplicationCommand, Result>
{
    public async Task<Result> Handle(SubmitHotelierApplicationCommand request, CancellationToken cancellationToken)
    {
        if (request.ApplicantId is null || request.ApplicantId.Value == Guid.Empty)
            return Result.Failure(new Error("User.Unauthorized", "An authenticated applicant is required."));
        if (await hotelierApplicationRepository.HasPendingForApplicantAsync(request.ApplicantId, cancellationToken))
            return Result.Failure(new Error("HotelierApplication.AlreadyPending", "You already have a pending hotelier application."));
        var latest = await hotelierApplicationRepository.GetLatestByApplicantAsync(request.ApplicantId, cancellationToken);
        if (latest is not null && latest.Status != Accommodations.Domain.Entities.HotelierApplications.HotelierApplicationStatus.Rejected)
            return Result.Failure(new Error("HotelierApplication.ResubmissionNotAllowed", "Only rejected applications can be submitted again."));
        var application = HotelierApplication.Create(request.ApplicantId, request.LegalBusinessName, request.RegistrationNumber,
            request.TaxNumber, request.BusinessEmail, request.BusinessPhoneNumber, request.FirstPropertyName, request.FirstPropertyAddress);
        if (application.IsFailure) return Result.Failure(application.Error);
        await hotelierApplicationRepository.AddAsync(application.Value, cancellationToken);
        return Result.Success();
    }
}
