using MediatR;

namespace Reviews.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
