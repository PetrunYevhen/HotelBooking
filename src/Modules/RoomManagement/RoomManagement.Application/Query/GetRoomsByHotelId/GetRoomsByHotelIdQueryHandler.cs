using AutoMapper;
using DTO.DTOs.RoomDto;
using MediatR;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetRoomsByHotelId;

public class GetRoomsByHotelIdQueryHandler : IRequestHandler<GetRoomsByHotelIdQuery, List<RoomDto>>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
    private readonly IMapper _mapper;

    public GetRoomsByHotelIdQueryHandler(IRoomManagmentReadRepository roomManagmentReadRepository, IMapper mapper)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
        _mapper = mapper;
    }

    public async Task<List<RoomDto>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomManagmentReadRepository.GetRoomsByHotelIdAsync(request.HotelId, cancellationToken);

        if (rooms == null) throw new Exception("No rooms found");
        
        return _mapper.Map<List<RoomDto>>(rooms);
    }
}