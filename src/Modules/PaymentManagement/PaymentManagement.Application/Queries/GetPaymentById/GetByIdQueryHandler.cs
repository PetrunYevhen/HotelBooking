using MediatR;
using PaymentManagement.Domain.Entities;
using PaymentManagement.Domain.RepositiryContracts;

namespace PaymentManagement.Application.Queries.GetPaymentById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Payment>
{
    private readonly IPaymentReadRepository _paymentReadRepository;

    public GetByIdQueryHandler(IPaymentReadRepository paymentReadRepository)
    {
        _paymentReadRepository = paymentReadRepository;
    }

    public async Task<Payment> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentReadRepository.GetByIdAsync(paymentId);
        return payment;
    }
}