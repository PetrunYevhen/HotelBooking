using BuildingBlock.Domain;
using SharedKernel.HotelRelations;

namespace HotelManagement.Domain.Entities.Events;

public class GetMinPriceRoomDomanEvent : DomainEventBase
{
    public HotelRooms HotelRooms { get; }
    public decimal MinPrice { get; }
    
    public GetMinPriceRoomDomanEvent(HotelRooms hotelRooms, decimal minPrice)
    {
        HotelRooms = hotelRooms;
        MinPrice = minPrice;
    }
}