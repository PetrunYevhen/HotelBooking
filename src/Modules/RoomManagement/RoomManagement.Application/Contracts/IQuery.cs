using MediatR;

namespace RoomManagment.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}