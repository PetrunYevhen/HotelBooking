using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetMinPrice;

public class GetMinPriceQuery : QueryBase<decimal>
{
    public Guid HotelId { get; set; }
    
    public GetMinPriceQuery(Guid id)
    {
        HotelId = id;
    }
}