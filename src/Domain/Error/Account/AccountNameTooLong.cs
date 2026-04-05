namespace Domain.Error.Account;

public record AccountNameTooLong()
    : AccountError("Account.Name.TooLong", "Account name is too long (max 128 characters)");
