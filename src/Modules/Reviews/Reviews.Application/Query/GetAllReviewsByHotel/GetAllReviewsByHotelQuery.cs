using Reviews.Application.Contracts;

namespace Reviews.Application.Query.GetAllReviewsByHotel;

public class GetAllReviewsByHotelQuery : QueryBase<List<ReviewDto>>
{
    public GetAllReviewsByHotelQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }

    public Guid HotelId { get; set; }
}