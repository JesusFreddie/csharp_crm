namespace Domain.Error.Counterparty;

public record NotArchived() : CounterpartyError("account.not_archived", "Account is not archived");
