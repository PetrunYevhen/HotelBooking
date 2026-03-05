using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.CalculatePrice;

public class CalculatePriceQuery : QueryBase<decimal>
{
    public CalculatePriceQuery(Guid roomId, DateTime checkIn, DateTime checkOut)
    {
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
}