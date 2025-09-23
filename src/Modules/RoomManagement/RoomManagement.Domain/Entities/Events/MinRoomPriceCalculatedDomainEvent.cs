using BuildingBlock.Domain;
using SharedKernel.HotelRelations;

namespace RoomManagment.Domain.Entities.Events;

public class MinRoomPriceCalculatedDomainEvent : DomainEventBase
{
    public HotelRooms HotelRooms { get; }
    public decimal MinPrice { get; }
    
    public MinRoomPriceCalculatedDomainEvent(HotelRooms roomReference, decimal minPrice)
    {
        HotelRooms = roomReference;
        MinPrice = minPrice;
    }
}