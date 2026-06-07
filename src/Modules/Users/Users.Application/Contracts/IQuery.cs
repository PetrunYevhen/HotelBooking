using MediatR;

namespace Users.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}