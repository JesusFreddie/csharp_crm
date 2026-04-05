namespace Domain.Error.Account;

public record AccountNameRequired()
    : AccountError("Account.Name.Required", "Account name is required");
