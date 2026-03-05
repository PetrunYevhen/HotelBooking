using MediatR;

namespace Facilities.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}