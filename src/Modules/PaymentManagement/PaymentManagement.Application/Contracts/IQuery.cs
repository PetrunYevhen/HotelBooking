using MediatR;

namespace PaymentManagement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}