namespace SharedKernel.Contracts;

public sealed record RoomBookingDetails(Guid RoomId, Guid HotelId, int Capacity, bool IsActive);
