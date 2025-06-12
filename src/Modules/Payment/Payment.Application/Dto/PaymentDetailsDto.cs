namespace Payment.Application.Dto;

public class PaymentDetailsDto
{
    public string Title = "Payment Details";
    
    public decimal PriceForAllNights { get; set; }
    public decimal CleaningFee { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal TotalAmount  => CleaningFee + ServiceFee;
}