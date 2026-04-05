namespace Domain.Error.Counterparty;

public record AlreadyArchived() : CounterpartyError("account.already_archived", "The account is already archived");