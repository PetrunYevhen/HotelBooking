namespace Accommodations.Application.Query.Hotels.GetCancellationPolicy;

public class HotelCancellationPolicyDto
{
    public int Type { get; set; }
    public int? DeadlineDays { get; set; }
    public double? PercentagePenalty { get; set; }
}
