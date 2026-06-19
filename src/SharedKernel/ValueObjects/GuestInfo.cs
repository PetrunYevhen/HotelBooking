using System.Text.RegularExpressions;
using BuildingBlock.Domain;

namespace SharedKernel.ValueObjects;

public class GuestInfo : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    private static readonly Regex PhoneRegex = new(
        @"^\+[1-9]\d{1,14}$",
        RegexOptions.Compiled);

    private GuestInfo(string firstName, string lastName, string email, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }

    public static Result<GuestInfo> Create(string firstName, string lastName, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<GuestInfo>(new Error("GuestInfo.InvalidFirstName", "First name is required."));

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<GuestInfo>(new Error("GuestInfo.InvalidLastName", "Last name is required."));

        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            return Result.Failure<GuestInfo>(new Error("GuestInfo.InvalidEmail", "Invalid email format."));

        if (string.IsNullOrWhiteSpace(phoneNumber) || !PhoneRegex.IsMatch(phoneNumber))
            return Result.Failure<GuestInfo>(new Error("GuestInfo.InvalidPhone", "Phone must be E.164 format (e.g. +380501234567)."));

        return Result.Success(new GuestInfo(firstName, lastName, email.ToLowerInvariant(), phoneNumber));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Email;
        yield return PhoneNumber;
    }
}