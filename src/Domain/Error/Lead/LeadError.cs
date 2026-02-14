namespace Domain.Error.Lead;

public record LeadError(string Code, string Message) : DomainError(Code, Message);