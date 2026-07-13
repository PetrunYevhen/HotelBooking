using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Hotels.GetCheckOutHours;

public class GetHotelCheckOutHoursQuery : QueryBase<int>
{
    public Guid HotelId { get; set; }
}