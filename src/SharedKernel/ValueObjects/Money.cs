using BuildingBlock.Domain;

namespace SharedKernel.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
         Amount = amount;
         Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount <= 0)
            return Result.Failure<Money>
                (new Error("Money.InvalidAmount", "Amount must be greater than zero."));

        if (string.IsNullOrWhiteSpace(currency))
            return Result.Failure<Money>
                (new("Money.InvalidCurrency", "Currency must be specified."));
        
        if (currency.Length != 3)
            return Result.Failure<Money>(
                new("Money.InvalidCurrencyFormat", "Currency must be ISO 4217 (3 chars)."));
        
        return Result.Success(new Money(amount, currency.ToUpperInvariant()));
    }
    
    public Result<Money> Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot add {Currency} and {other.Currency}.");
        return Result.Success(new Money(Amount + other.Amount, Currency));
    }

    public Result<Money> Multiply(decimal multiplier)
    {
        if (multiplier < 0) throw new ArgumentException("Multiplier cannot be negative.");
        return Result.Success(new Money(Amount * multiplier, Currency));
    }

    public Result<Money> Subtract(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot subtract {Currency} and {other.Currency}.");
        if (Amount - other.Amount < 0)
            throw new InvalidOperationException("Result cannot be negative.");
        return Result.Success(new Money(Amount - other.Amount, Currency));
    }
    
    public static Money Zero(string currency) => new(0, currency);
    public override string ToString() => $"{Amount:F2} {Currency}";
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}