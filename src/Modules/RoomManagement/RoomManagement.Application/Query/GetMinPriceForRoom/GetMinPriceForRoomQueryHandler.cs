// using MediatR;
// using RoomManagement.Domain.RepositoryContract;
//
// namespace RoomManagement.Application.Query.GetMinPriceForRoom;
//
// public class GetMinPriceForRoomQueryHandler : IRequestHandler<GetMinPriceForRoomQuery, decimal>
// {
//     private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
//
//     public GetMinPriceForRoomQueryHandler(IRoomManagmentReadRepository roomManagmentReadRepository)
//     {
//         _roomManagmentReadRepository = roomManagmentReadRepository;
//     }
//
//     public async Task<decimal> Handle(GetMinPriceForRoomQuery request, CancellationToken cancellationToken)
//     {
//         return await _roomManagmentReadRepository.GetMinRoomPriceInHotelAsync(request.HotelId, cancellationToken);
//     }
// }