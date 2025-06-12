namespace Payment.Application.Dto;

public class PaymentMethodDto
{
    public string Title = "Pay With";
    public Guid PaymentMethodId { get; set; }
    public string MethodType { get; set; }
}