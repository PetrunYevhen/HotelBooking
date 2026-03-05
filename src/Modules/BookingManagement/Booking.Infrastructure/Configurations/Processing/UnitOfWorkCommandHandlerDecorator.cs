using BookingManagement.Application.Contracts;
using Infrastructure.UnitOfWork;
using MediatR;

namespace BookingManagement.Infrastructure.Configurations.Processing;

public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T> where T : ICommand
{
    private readonly IRequestHandler<T> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(IRequestHandler<T> commandHandler, IUnitOfWork unitOfWork)
    {
        _decorated = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        await _decorated.Handle(request, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}