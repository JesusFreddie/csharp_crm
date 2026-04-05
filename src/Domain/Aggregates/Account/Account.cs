using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Error;
using Domain.Error.Account;

namespace Domain.Aggregates.Account;

public class Account : BaseEntity, IArchivable
{
    const int MaxNameLength = 128;

    public string Name { get; private set; }
    public bool IsArchived { get; private set; } = false;

    private Account(Guid id, string name, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Account, AccountError> Create(
        Guid id,
        string name,
        DateTime createdAt,
        DateTime updatedAt)
    {
        name = name.Trim();

        if (string.IsNullOrWhiteSpace(name))
            return new AccountNameRequired();
        if (name.Length > MaxNameLength)
            return new AccountNameTooLong();

        return new Account(id, name, createdAt, updatedAt);
    }

    public UnitResult<DomainError> Archive()
    {
        if (IsArchived) return new Error.Account.AlreadyArchived();

        IsArchived = true;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Restore()
    {
        if (!IsArchived) return new Error.Account.NotArchived();

        IsArchived = false;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> SetName(string name)
    {
        name = name.Trim();

        if (string.IsNullOrWhiteSpace(name))
            return new AccountNameRequired();
        if (name.Length > MaxNameLength)
            return new AccountNameTooLong();

        Name = name;
        return UnitResult.Success<DomainError>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Account other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}