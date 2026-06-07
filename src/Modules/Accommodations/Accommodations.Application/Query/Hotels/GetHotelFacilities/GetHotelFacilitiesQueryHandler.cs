// using Accommodations.Domain.Entities;
// using Accommodations.Domain.RepositoryContract;
// using MediatR;
//
// namespace HotelManagement.Application.Query.GetAllFacilitiesInHotel;
//
// public class GetFacilitiesQueryHandler : IRequestHandler<GetFacilitiesQuery, List<Guid>>
// {
//     private readonly IHotelReadRepository _hotelReadRepository;
//
//     public GetFacilitiesQueryHandler(IHotelReadRepository hotelReadRepository)
//     {
//         _hotelReadRepository = hotelReadRepository;
//     }
//
//     public async Task<List<Guid>> Handle(GetFacilitiesQuery request, CancellationToken cancellationToken)
//     {
//         var Id = new Id(request.Id);
//         var facilityIds = 
//             await _hotelReadRepository.GetFacilityIdsByIdAsync(Id, cancellationToken);
//         
//         if(facilityIds.Count == 0)
//             throw new Exception($"No facilities found for hotel with id {request.Id}");
//         
//         return facilityIds;
//     }
// }