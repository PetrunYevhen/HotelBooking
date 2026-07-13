using MediatR;

namespace Notifications.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
