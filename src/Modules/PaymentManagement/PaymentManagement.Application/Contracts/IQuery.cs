using MediatR;

namespace PaymantManagement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}