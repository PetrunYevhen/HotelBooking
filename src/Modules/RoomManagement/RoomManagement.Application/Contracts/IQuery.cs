using MediatR;

namespace RoomManagement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}