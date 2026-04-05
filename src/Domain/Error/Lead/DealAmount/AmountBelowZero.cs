namespace Domain.Error.Lead.DealAmount;

public record AmountBelowZero() : DealAmountError("lead.deal_amount.amount_below_zero", "Amount can't be below zero");