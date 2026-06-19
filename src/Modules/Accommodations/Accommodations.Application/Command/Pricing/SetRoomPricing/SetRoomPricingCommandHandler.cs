using Accommodations.Application.Services.Pricing;
using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Pricing;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.Pricing.SetRoomPricing;

public class SetRoomPricingCommandHandler : IRequestHandler<SetRoomPricingCommand, Result>
{
    private readonly IPricingRepository _pricingRepository;
    private readonly IRoomRepository _roomRepository;

    public SetRoomPricingCommandHandler(IPricingRepository pricingRepository, IRoomRepository roomRepository)
    {
        _pricingRepository = pricingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(SetRoomPricingCommand request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId); 
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (room == null)
            return Result.Failure(new Error("Room.NotFound", "Room not found"));       
        
        var priceResult = Money.Create(request.Price, request.Currency);
        var datesResult = DateRange.Create(
            DateTime.SpecifyKind(request.ValidFrom, DateTimeKind.Utc),
            DateTime.SpecifyKind(request.ValidTo, DateTimeKind.Utc));
        if (datesResult.IsFailure)
            return Result.Failure(datesResult.Error);

        
        var pricingResult = Domain.Entities.Pricing.Pricing.CreatePromotional(
            roomId,
            priceResult.Value,
            datesResult.Value);
        if (pricingResult.IsFailure)
            return Result.Failure(pricingResult.Error);

        await _pricingRepository.AddAsync(pricingResult.Value, cancellationToken);

        return Result.Success();
    }
}