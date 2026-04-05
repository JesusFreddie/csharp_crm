namespace Domain.Error.Counterparty;

public record CounterpartyNameRequired()
    : CounterpartyError("Account.Name.Required", "Account name is required");
