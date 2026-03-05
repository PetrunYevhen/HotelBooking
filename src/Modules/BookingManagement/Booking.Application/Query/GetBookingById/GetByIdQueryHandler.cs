using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using MediatR;

namespace BookingManagement.Application.Query.GetBookingById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BookingDto>
{
    private readonly IBookingReadRepository _bookingReadRepository;

    public GetByIdQueryHandler(IBookingReadRepository bookingReadRepository)
    {
        _bookingReadRepository = bookingReadRepository;
    }

    public async Task<BookingDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingReadRepository.GetByIdAsync(bookingId, cancellationToken);

        return new BookingDto(
            bookingId.Value,
            booking.RoomId,
            booking.HotelId,
            booking.CheckInDate,
            booking.CheckOutDate,
            booking.TotalPrice,
            booking.Status.ToString(),
            booking.CreatedAt);
    }
}