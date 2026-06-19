using Accommodations.Application.Contracts;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Query.Rooms.GetRoomPrice;

public class GetRoomPriceQuery : QueryBase<Result<Money>>
{
    public Guid RoomId { get; }
    public DateTime CheckIn { get; }
    public DateTime CheckOut { get; }

    public GetRoomPriceQuery(Guid roomId, DateTime checkIn, DateTime checkOut)
    {
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}