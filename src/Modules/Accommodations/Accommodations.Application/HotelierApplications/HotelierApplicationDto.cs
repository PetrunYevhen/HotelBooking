namespace Accommodations.Application.HotelierApplications;

public sealed class HotelierApplicationDto
{
    public Guid HotelierApplicationId { get; init; }
    public Guid ApplicantId { get; init; }
    public string ApplicantUsername { get; init; } = string.Empty;
    public string ApplicantEmail { get; init; } = string.Empty;
    public string LegalBusinessName { get; init; } = string.Empty;
    public string RegistrationNumber { get; init; } = string.Empty;
    public string? TaxNumber { get; init; }
    public string BusinessEmail { get; init; } = string.Empty;
    public string BusinessPhoneNumber { get; init; } = string.Empty;
    public string FirstPropertyName { get; init; } = string.Empty;
    public string FirstPropertyAddress { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime SubmittedAt { get; init; }
    public DateTime? ReviewedAt { get; init; }
    public Guid? ReviewedByUserId { get; init; }
    public string? RejectionReason { get; init; }
}
