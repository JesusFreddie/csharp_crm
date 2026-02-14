namespace Application.Error;

public record NotFound() : AppError("not_found", "Not found");