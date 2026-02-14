namespace Domain.Error.Account;

public record NotArchived() : AccountError("account.not_archived", "Account is not archived");
