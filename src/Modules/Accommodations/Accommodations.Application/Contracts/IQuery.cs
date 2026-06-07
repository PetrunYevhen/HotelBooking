using MediatR;

namespace Accommodations.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}