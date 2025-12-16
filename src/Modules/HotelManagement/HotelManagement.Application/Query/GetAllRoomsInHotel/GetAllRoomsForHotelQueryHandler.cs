// using HotelManagement.Application.CachingContract;
// using HotelManagement.Domain.Entities;
// using HotelManagement.Domain.RepositoryContract;
// using MediatR;
//
// namespace HotelManagement.Application.Query.GetAllRoomsInHotel;
//
// public class GetAllRoomsForHotelQueryHandler : IRequestHandler<GetAllRoomsForHotelQuery, List<Guid>>
// {
//  private readonly IHotelReadRepository _hotelReadRepository;
//
//  public GetAllRoomsForHotelQueryHandler(IHotelReadRepository hotelRepository, IHotelRoomsCache hotelRoomsCache)
//  {
//      _hotelReadRepository = hotelRepository;
//  }
//
//  public async Task<List<Guid>> Handle(GetAllRoomsForHotelQuery request, CancellationToken cancellationToken)
//  {
//      
//          var hotelId = new HotelId(request.HotelId);
//          var rooms = await _hotelReadRepository.GetHotelByIdAsync(hotelId, cancellationToken);
//
//          if (rooms == null || !rooms.Any())
//          {
//              throw new InvalidOperationException($"No rooms found for hotel with id {request.HotelId}");
//          }
//
//          return rooms;
//      
// }