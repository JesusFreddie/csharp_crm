namespace Domain.Error.Account;

public record AlreadyArchived() : AccountError("account.already_archived", "The account is already archived");