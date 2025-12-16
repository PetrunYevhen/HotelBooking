// using HotelManagement.Domain.Entities;
// using HotelManagement.Domain.RepositoryContract;
// using MediatR;
//
// namespace HotelManagement.Application.Query.GetAllFacilitiesInHotel;
//
// public class GetAllFacilitiesInHotelQueryHandler : IRequestHandler<GetAllFacilitiesInHotelQuery, List<Guid>>
// {
//     private readonly IHotelReadRepository _hotelReadRepository;
//
//     public GetAllFacilitiesInHotelQueryHandler(IHotelReadRepository hotelReadRepository)
//     {
//         _hotelReadRepository = hotelReadRepository;
//     }
//
//     public async Task<List<Guid>> Handle(GetAllFacilitiesInHotelQuery request, CancellationToken cancellationToken)
//     {
//         var hotelId = new HotelId(request.HotelId);
//         var facilityIds = 
//             await _hotelReadRepository.GetFacilityIdsByHotelIdAsync(hotelId, cancellationToken);
//         
//         if(facilityIds.Count == 0)
//             throw new Exception($"No facilities found for hotel with id {request.HotelId}");
//         
//         return facilityIds;
//     }
// }