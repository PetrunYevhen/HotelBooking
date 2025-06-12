using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.RepositoryContracts;
using MediatR;
using Serilog;

namespace Application.Command.RequestToBook;

public class RequestToBookCommandHandler : IRequestHandler<RequestToBookCommand, Reservation>
{
    private readonly ILogger _logger;
    private readonly IReservationWriteRepository _reservationWriteRepository;
    
    public RequestToBookCommandHandler(ILogger logger, IReservationWriteRepository reservationWriteRepository)
    {
        _logger = logger;
        _reservationWriteRepository = reservationWriteRepository;
    }
    
    public async Task<Reservation> Handle(RequestToBookCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(
            "Handling RequestToBookCommand for GuestId={GuestId}, RoomId={RoomId}, Dates={StartDate}-{EndDate}",
            request.GuestId, request.RoomId, request.FromDate, request.ToDate);
        
        var requestToBook = new Reservation
        (
            new ReservationId(Guid.NewGuid()),
            request.GuestId,
            request.RoomId,
            request.TotalPayment,
            request.FromDate,
            request.ToDate,
            ReservationStatus.Pending
        );
        
        _logger.Information(
            "Created new reservation with Id={ReservationId} for GuestId={GuestId}, RoomId={RoomId}, Dates={StartDate}-{EndDate}",
            requestToBook.ReservationId, request.GuestId, request.RoomId, request.FromDate, request.ToDate);
        
        await _reservationWriteRepository.AddReservationAsync(requestToBook, cancellationToken);
        
        return requestToBook;
    }
} 