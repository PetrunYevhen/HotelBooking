using SharedKernel.Contracts;
using BuildingBlock.Domain;
using Accommodations.Application.Contracts;

namespace Accommodations.Application.HotelierApplications;

public sealed class SubmitHotelierApplicationCommand : CommandBase<Result>
{
    public AccountId ApplicantId { get; init; } = null!;
    public string LegalBusinessName { get; init; } = string.Empty;
    public string RegistrationNumber { get; init; } = string.Empty;
    public string? TaxNumber { get; init; }
    public string BusinessEmail { get; init; } = string.Empty;
    public string BusinessPhoneNumber { get; init; } = string.Empty;
    public string FirstPropertyName { get; init; } = string.Empty;
    public string FirstPropertyAddress { get; init; } = string.Empty;
}
