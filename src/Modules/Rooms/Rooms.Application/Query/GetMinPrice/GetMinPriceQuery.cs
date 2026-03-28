using Rooms.Application.Contracts;

namespace Rooms.Application.Query.GetMinPrice;

public class GetMinPriceQuery : QueryBase<decimal>
{
    public Guid HotelId { get; set; }
    
    public GetMinPriceQuery(Guid id)
    {
        HotelId = id;
    }
}