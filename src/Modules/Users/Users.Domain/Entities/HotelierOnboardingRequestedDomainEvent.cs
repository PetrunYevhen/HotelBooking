using BuildingBlock.Domain.Events;
using SharedKernel.Contracts;

namespace Users.Domain.Entities;

public sealed class HotelierOnboardingRequestedDomainEvent(UserId applicantId, HotelierApplicationDetails details) : DomainEventBase
{
    public UserId ApplicantId { get; } = applicantId;
    public HotelierApplicationDetails Details { get; } = details;
}
