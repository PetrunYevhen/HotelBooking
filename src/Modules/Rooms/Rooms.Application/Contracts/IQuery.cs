using MediatR;

namespace Rooms.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}