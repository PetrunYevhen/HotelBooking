using BuildingBlock.Domain;

namespace SharedKernel.Contracts;

public sealed record HotelierApplicationDetails(string LegalBusinessName, string RegistrationNumber, string? TaxNumber,
    string BusinessEmail, string BusinessPhoneNumber, string FirstPropertyName, string FirstPropertyAddress)
{
    public Result Validate()
    {
        var fields = new[] { (LegalBusinessName, 200), (RegistrationNumber, 100), (BusinessEmail, 320),
            (BusinessPhoneNumber, 32), (FirstPropertyName, 200), (FirstPropertyAddress, 500) };
        if (fields.Any(x => string.IsNullOrWhiteSpace(x.Item1) || x.Item1.Trim().Length > x.Item2)
            || TaxNumber?.Trim().Length > 100)
            return Result.Failure(new Error("HotelierApplication.InvalidDetails", "Business details are missing or exceed the allowed length."));
        if (!System.Net.Mail.MailAddress.TryCreate(BusinessEmail.Trim(), out _))
            return Result.Failure(new Error("HotelierApplication.InvalidBusinessEmail", "A valid business email is required."));
        return Result.Success();
    }
}

public sealed record HotelierOnboardingRequested(Guid ApplicantId, HotelierApplicationDetails Details);
public sealed record HotelierApplicationApproved(Guid ApplicantId);
