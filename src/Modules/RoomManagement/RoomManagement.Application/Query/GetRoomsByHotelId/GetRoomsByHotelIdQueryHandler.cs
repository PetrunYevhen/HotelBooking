using AutoMapper;
using DTO.DTOs.RoomDto;
using MediatR;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetRoomsByHotelId;

public class GetRoomsByIdQueryHandler : IRequestHandler<GetRoomsByIdQuery, List<RoomDto>>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;
    private readonly IMapper _mapper;

    public GetRoomsByIdQueryHandler(IRoomManagementReadRepository roomManagementReadRepository, IMapper mapper)
    {
        _roomReadRepository = roomManagementReadRepository;
        _mapper = mapper;
    }

    public async Task<List<RoomDto>> Handle(GetRoomsByIdQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomReadRepository.GetByHotelIdAsync(request.HotelId, cancellationToken);

        // if (rooms == null || !rooms.Any()) return new List<RoomDto>();
        
        return _mapper.Map<List<RoomDto>>(rooms);
    }
}