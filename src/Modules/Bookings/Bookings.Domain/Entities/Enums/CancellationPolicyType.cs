namespace Bookings.Domain.Entities.Enums;

/// <summary>
/// Mirrors Accommodations.Domain.Entities.Hotels.Enums.CancellationPolicyType by ordinal contract.
/// Bookings module cannot reference Accommodations directly (module isolation).
/// </summary>
public enum CancellationPolicyType
{
    FreeCancellation = 1,
    PartialRefund = 2,
    NonRefundable = 3
}
