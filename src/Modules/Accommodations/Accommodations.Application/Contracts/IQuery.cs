using MediatR;

namespace Hotels.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}