using MediatR;

namespace Payments.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}