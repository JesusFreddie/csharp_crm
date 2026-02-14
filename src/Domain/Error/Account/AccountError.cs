namespace Domain.Error.Account;

public record AccountError(string Code, string Message) : DomainError(Code, Message);
