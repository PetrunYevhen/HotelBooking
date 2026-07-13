using BuildingBlock.Domain;

namespace Accommodations.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public Address(string city, string street, string country, string postalCode)
    {
        City = city;
        Street = street;
        Country = country;
        PostalCode = postalCode;
    }

    public string City { get; }
    public string Street { get; }
    public string Country { get; }
    public string PostalCode { get; }
    
    public static Result<Address> Create(string city, string street, string country, string postalCode)
    {
        if(string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>(new Error("Address.City", "City is required"));
        if(string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>(new Error("Address.Street", "Street is required"));
        if(string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>(new Error("Address.Country", "Country is required"));
        
        return Result.Success(new Address(city, street, country, postalCode));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return Country;
        yield return PostalCode;
    }
}