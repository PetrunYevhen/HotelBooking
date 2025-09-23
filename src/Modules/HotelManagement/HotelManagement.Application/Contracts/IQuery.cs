using MediatR;

namespace HotelManagement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}