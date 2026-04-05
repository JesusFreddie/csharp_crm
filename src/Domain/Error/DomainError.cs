using Core.Error;

namespace Domain.Error;

public abstract record DomainError(string Code, string Message) : BaseError(Code, Message);
