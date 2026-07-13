using System.Text.RegularExpressions;
using BuildingBlock.Domain;

namespace Users.Domain.ValueObjects;

public class UserPersonalInfo : ValueObject
{
    private static readonly Regex PhoneRegex = new(
        @"^\+[1-9]\d{1,14}$",
        RegexOptions.Compiled);
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    
    private UserPersonalInfo(string firstName, string lastName, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public static Result<UserPersonalInfo> Create(string firstName, string lastName, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<UserPersonalInfo>(new Error("UserPersonalInfo.InvalidFirstName", "First name is required."));

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<UserPersonalInfo>(new Error("UserPersonalInfo.InvalidLastName", "Last name is required."));

        if (string.IsNullOrWhiteSpace(phoneNumber) || !PhoneRegex.IsMatch(phoneNumber))
            return Result.Failure<UserPersonalInfo>(new Error("UserPersonalInfo.InvalidPhone", "Phone must be E.164 format (e.g. +380501234567)."));
        
        return Result.Success(new UserPersonalInfo(firstName, lastName, phoneNumber));
    }
    
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return PhoneNumber;
    }
}