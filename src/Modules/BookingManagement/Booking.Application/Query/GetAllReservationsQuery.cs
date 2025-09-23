using BookingManagement.Domain.Entities;
using MediatR;

namespace BookingManagement.Application.Query;

public class GetAllReservationsQuery : IRequest<List<Booking>>
{
    
}