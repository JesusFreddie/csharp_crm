using CSharpFunctionalExtensions;
using Domain.Error.Lead.DealAmount;

namespace Domain.ValueObjects;

public record DealAmount
{
    public decimal Amount { get; init; }
    public PricingMode Mode { get; init; }
    
    private DealAmount(decimal amount, PricingMode mode)
    {
        Amount = amount;
        Mode = mode;
    }

    public static Result<DealAmount, DealAmountError> Manual(decimal amount)
    {
        if (amount <= 0m) return new AmountBelowZero();
        return new DealAmount(amount, PricingMode.Manual);
    }

    public static Result<DealAmount, DealAmountError> Calculated(decimal amount)
    {
        if  (amount <= 0m) return new AmountBelowZero();
        return new DealAmount(amount, PricingMode.Calculated);
    }

    public static DealAmount Empty() => new DealAmount(0, PricingMode.Manual);
}

public enum PricingMode
{
    Manual,
    Calculated
}