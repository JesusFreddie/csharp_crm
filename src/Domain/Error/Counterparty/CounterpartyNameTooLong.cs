namespace Domain.Error.Counterparty;

public record CounterpartyNameTooLong()
    : CounterpartyError("Account.Name.TooLong", "Account name is too long (max 128 characters)");
