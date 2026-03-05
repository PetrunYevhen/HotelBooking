using BookingManagement.Application.Gateways;
using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using MediatR;

namespace BookingManagement.Application.Command.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingWriteRepository _bookingWriteRepository;
    private readonly IBookingReadRepository _bookingReadRepository;
    private readonly IRoomGateway _roomGateway;
    
    public CreateBookingCommandHandler(IBookingWriteRepository bookingWriteRepository, IRoomGateway roomGateway, IBookingReadRepository bookingReadRepository)
    {
        _bookingWriteRepository = bookingWriteRepository;
        _roomGateway = roomGateway;
        _bookingReadRepository = bookingReadRepository;
    }

    public async Task<Guid> Handle(CreateBookingCommand request,CancellationToken cancellationToken)
    {
        var isAlreadyBooked = await _bookingReadRepository.IsBookedInRangeAsync(
            request.RoomId, 
            request.CheckInDate, 
            request.CheckOutDate, 
            cancellationToken);
        
        if (isAlreadyBooked)
            throw new InvalidOperationException("Booking already exists");
        
        var totalPrice = await _roomGateway.GetPriceAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, cancellationToken);

        var booking = Booking.CreateNew(
            request.HotelId, 
            request.RoomId, 
            request.CheckInDate, 
            request.CheckOutDate,
            totalPrice);
        
        booking.Pending();
        
        await _bookingWriteRepository.CreateAsync(booking, cancellationToken);

        return booking.BookingId.Value;
    }
}