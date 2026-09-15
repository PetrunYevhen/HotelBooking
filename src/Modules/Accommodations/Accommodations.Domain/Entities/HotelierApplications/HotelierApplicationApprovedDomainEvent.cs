using BuildingBlock.Domain.Events;
using SharedKernel.Contracts;

namespace Accommodations.Domain.Entities.HotelierApplications;

public sealed class HotelierApplicationApprovedDomainEvent(AccountId applicantId) : DomainEventBase
{
    public AccountId ApplicantId { get; } = applicantId;
}
