using Infrastructure.UnitOfWork;
using MediatR;
using PaymentManagement.Application.Contracts;

namespace PaymentManagement.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : IRequestHandler<T, TResult> 
 where T : ICommand<TResult>
{
    private readonly IRequestHandler<T, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerWithResultDecorator(IRequestHandler<T, TResult> decorated, IUnitOfWork unitOfWork)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(request, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}