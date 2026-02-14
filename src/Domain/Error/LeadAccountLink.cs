namespace Domain.Error;

public record LeadAccountLink(string Code, string Message) : DomainError(Code, Message);