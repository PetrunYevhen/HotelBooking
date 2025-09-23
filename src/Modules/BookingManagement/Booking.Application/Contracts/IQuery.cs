using MediatR;

namespace BookingManagement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}