using AutoMapper;
using MediatR;
using RoomManagment.Application.Dto;
using RoomManagment.Domain.RepositoryContract;

namespace Application.Query.GetRoomBookingDetails;

public class GetRoomBookingDetailsQueryHandler : IRequestHandler<GetRoomBookingDetailsQuery, RoomBookingDetailsDto>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
    private readonly IMapper _mapper;
    
    public GetRoomBookingDetailsQueryHandler(IRoomManagmentReadRepository roomManagmentReadRepository, 
        IMapper mapper)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
        _mapper = mapper;
    }
    
    public async Task<RoomBookingDetailsDto> Handle(GetRoomBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomManagmentReadRepository.GetRoomByIdAsync(request.RoomId);
        return _mapper.Map<RoomBookingDetailsDto>(room);
    }
}