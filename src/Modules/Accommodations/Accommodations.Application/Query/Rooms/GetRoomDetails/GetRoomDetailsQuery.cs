using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomBookingDetailsDto>
{
    public GetRoomDetailsQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; init; }
}