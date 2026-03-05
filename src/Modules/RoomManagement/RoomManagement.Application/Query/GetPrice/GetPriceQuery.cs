using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetPrice;

public class GetPriceQuery : QueryBase<decimal>
{
    public GetPriceQuery(Guid roomId)
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; set; }
    
    
}