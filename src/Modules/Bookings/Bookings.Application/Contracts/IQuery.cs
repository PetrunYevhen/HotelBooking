using MediatR;

namespace Bookings.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}