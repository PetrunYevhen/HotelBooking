using SharedKernel.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.HotelierApplications;

public sealed class HotelierApplication : Entity, IAggregateRoot
{
    public HotelierApplicationId HotelierApplicationId { get; private set; } = null!;
    public AccountId ApplicantId { get; private set; } = null!;
    public string LegalBusinessName { get; private set; } = string.Empty;
    public string RegistrationNumber { get; private set; } = string.Empty;
    public string? TaxNumber { get; private set; }
    public string BusinessEmail { get; private set; } = string.Empty;
    public string BusinessPhoneNumber { get; private set; } = string.Empty;
    public string FirstPropertyName { get; private set; } = string.Empty;
    public string FirstPropertyAddress { get; private set; } = string.Empty;
    public HotelierApplicationStatus Status { get; private set; }
    public DateTime SubmittedAt { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public AccountId? ReviewedByUserId { get; private set; }
    public string? RejectionReason { get; private set; }

    private HotelierApplication() { }

    private HotelierApplication(AccountId applicantId, string legalBusinessName, string registrationNumber, string? taxNumber,
        string businessEmail, string businessPhoneNumber, string firstPropertyName, string firstPropertyAddress)
    {
        HotelierApplicationId = new HotelierApplicationId(Guid.NewGuid());
        ApplicantId = applicantId;
        LegalBusinessName = legalBusinessName.Trim();
        RegistrationNumber = registrationNumber.Trim();
        TaxNumber = string.IsNullOrWhiteSpace(taxNumber) ? null : taxNumber.Trim();
        BusinessEmail = businessEmail.Trim().ToLowerInvariant();
        BusinessPhoneNumber = businessPhoneNumber.Trim();
        FirstPropertyName = firstPropertyName.Trim();
        FirstPropertyAddress = firstPropertyAddress.Trim();
        Status = HotelierApplicationStatus.Pending;
        SubmittedAt = DateTime.UtcNow;
    }

    public static Result<HotelierApplication> Create(AccountId applicantId, string legalBusinessName, string registrationNumber,
        string? taxNumber, string businessEmail, string businessPhoneNumber, string firstPropertyName, string firstPropertyAddress)
    {
        if (applicantId is null || applicantId.Value == Guid.Empty)
            return Result.Failure<HotelierApplication>(new Error("HotelierApplication.InvalidApplicant", "An applicant is required."));
        if (new[] { legalBusinessName, registrationNumber, businessEmail, businessPhoneNumber, firstPropertyName, firstPropertyAddress }
            .Any(string.IsNullOrWhiteSpace))
            return Result.Failure<HotelierApplication>(new Error("HotelierApplication.InvalidDetails", "All required business and property details must be provided."));
        if (!System.Net.Mail.MailAddress.TryCreate(businessEmail.Trim(), out _))
            return Result.Failure<HotelierApplication>(new Error("HotelierApplication.InvalidBusinessEmail", "A valid business email address is required."));
        var validation = new HotelierApplicationDetails(legalBusinessName, registrationNumber, taxNumber,
            businessEmail, businessPhoneNumber, firstPropertyName, firstPropertyAddress).Validate();
        if (validation.IsFailure) return Result.Failure<HotelierApplication>(validation.Error);

        return Result.Success(new HotelierApplication(applicantId, legalBusinessName, registrationNumber, taxNumber,
            businessEmail, businessPhoneNumber, firstPropertyName, firstPropertyAddress));
    }

    public Result Approve(AccountId reviewerId)
    {
        if (Status != HotelierApplicationStatus.Pending)
            return Result.Failure(new Error("HotelierApplication.NotPending", "Only pending applications can be approved."));
        Status = HotelierApplicationStatus.Approved;
        ReviewedAt = DateTime.UtcNow;
        ReviewedByUserId = reviewerId;
        RejectionReason = null;
        AddDomainEvent(new HotelierApplicationApprovedDomainEvent(ApplicantId));
        return Result.Success();
    }

    public Result Reject(AccountId reviewerId, string reason)
    {
        if (Status != HotelierApplicationStatus.Pending)
            return Result.Failure(new Error("HotelierApplication.NotPending", "Only pending applications can be rejected."));
        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(new Error("HotelierApplication.RejectionReasonRequired", "A rejection reason is required."));
        if (reason.Trim().Length > 1000)
            return Result.Failure(new Error("HotelierApplication.InvalidReason", "Rejection reason must not exceed 1000 characters."));
        Status = HotelierApplicationStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;
        ReviewedByUserId = reviewerId;
        RejectionReason = reason.Trim();
        return Result.Success();
    }
}
