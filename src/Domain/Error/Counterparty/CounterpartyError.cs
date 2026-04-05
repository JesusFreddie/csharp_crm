namespace Domain.Error.Counterparty;

public record CounterpartyError(string Code, string Message) : DomainError(Code, Message);
