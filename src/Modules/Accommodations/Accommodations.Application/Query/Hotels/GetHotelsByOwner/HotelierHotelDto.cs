namespace Accommodations.Application.Query.Hotels.GetHotelsByOwner;

public sealed record HotelierHotelDto(Guid HotelId, string Name, string City, string Country, Guid? OwnerUserId);
