namespace Domain.Error.Lead.DealAmount;

public record DealAmountError(string Code, string Message) : DomainError(Code, Message);