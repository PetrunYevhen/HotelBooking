// using MediatR;
// using RoomManagement.Domain.RepositoryContract;
//
// namespace RoomManagement.Application.Query.GetMinPriceRoom;
//
// public class GetMinPriceRoomQueryHandler : IRequestHandler<GetMinPriceRoomQuery, decimal>
// {
//     private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
//
//     public GetMinPriceRoomQueryHandler(IRoomManagmentReadRepository roomManagmentReadRepository)
//     {
//         _roomManagmentReadRepository = roomManagmentReadRepository;
//     }
//
//
//     public Task<decimal> Handle(GetMinPriceRoomQuery request, CancellationToken cancellationToken)
//     {
//         return _roomManagmentReadRepository.GetMinRoomPriceInHotelAsync(request.HotelId, cancellationToken);
//     }
// }