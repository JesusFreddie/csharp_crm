using Core.Error;

namespace Application.Error;

public abstract record AppError(string Code, string Message) : BaseError(Code, Message);