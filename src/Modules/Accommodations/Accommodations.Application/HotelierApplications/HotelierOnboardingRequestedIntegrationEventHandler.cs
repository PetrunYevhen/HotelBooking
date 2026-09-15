using Accommodations.Domain.Entities.HotelierApplications;
using Accommodations.Domain.RepositoryContract.HotelierApplications;
using Infrastructure.EventBus;
using MediatR;
using SharedKernel.Contracts;

namespace Accommodations.Application.HotelierApplications;

public sealed class HotelierOnboardingRequestedIntegrationEventHandler(IHotelierApplicationRepository hotelierApplicationRepository)
    : INotificationHandler<ContractIntegrationEvent<HotelierOnboardingRequested>>
{
    public async Task Handle(ContractIntegrationEvent<HotelierOnboardingRequested> notification, CancellationToken cancellationToken)
    {
        var applicantId = new AccountId(notification.Data.ApplicantId);
        // Registration initiates only the first application. Replays must not create a resubmission.
        if (await hotelierApplicationRepository.GetLatestByApplicantAsync(applicantId, cancellationToken) is not null) return;
        var details = notification.Data.Details;
        var application = HotelierApplication.Create(applicantId, details.LegalBusinessName, details.RegistrationNumber,
            details.TaxNumber, details.BusinessEmail, details.BusinessPhoneNumber, details.FirstPropertyName, details.FirstPropertyAddress);
        if (application.IsFailure) throw new InvalidOperationException(application.Error.Code);
        await hotelierApplicationRepository.AddAsync(application.Value, cancellationToken);
    }
}
