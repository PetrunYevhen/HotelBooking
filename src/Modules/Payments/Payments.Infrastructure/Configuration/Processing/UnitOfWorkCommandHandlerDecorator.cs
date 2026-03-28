using Infrastructure.UnitOfWork;
using MediatR;
using Payments.Application.Contracts;

namespace Payments.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T> where T : ICommand, IRequest
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