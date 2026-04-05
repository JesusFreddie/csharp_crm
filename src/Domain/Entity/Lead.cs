using CSharpFunctionalExtensions;
using Domain.Error;
using Domain.Error.Lead;

namespace Domain.Entity;

public class Lead : BaseEntity, IArchivable
{
    const int MaxNameLength = 128;
    const int MaxDescriptionLength = 1000;

    public string Name { get; private set;  }
    public string Description { get; private set; }
    
    public bool IsArchived { get; private set; } = false;
    public bool IsDeleted { get; private set; } = false;

    public DealAmount Amount { get; private init; }
    
    public static Result<Lead, LeadError> Create(
        Guid id, 
        string name, 
        string description, 
        DealAmount amount,
        DateTime createAt, 
        DateTime updateAt)
    {
        name = name.Trim();
        description = description?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name)) 
            return new NameRequired();
        if (name.Length > MaxNameLength) 
            return new NameTooLong();
        if (description.Length > MaxDescriptionLength) 
            return new DescriptionTooLong();
        
        return new Lead(id, name, description, amount, createAt, updateAt);
    }


    public UnitResult<DomainError> Archive()
    {
        if (IsDeleted)
            return new CannotModifyDeleted();
        if (IsArchived)
            return new AlreadyArchived();

        IsArchived = true;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Restore()
    {
        if (IsDeleted)
            return new CannotModifyDeleted();
        if (!IsArchived)
            return new NotArchived();

        IsArchived = false;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Delete()
    {
        if (IsDeleted)
            return new AlreadyDeleted();

        IsDeleted = true;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> SetName(string name)
    {
        if (IsDeleted)
            return new CannotModifyDeleted();
        if (IsArchived)
            return new CannotModifyArchived();

        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return new NameRequired();
        if (name.Length > MaxNameLength)
            return new NameTooLong();

        Name = name;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> SetDescription(string description)
    {
        if (IsDeleted)
            return new CannotModifyDeleted();
        if (IsArchived)
            return new CannotModifyArchived();

        description = description?.Trim() ?? string.Empty;
        if (description.Length > MaxDescriptionLength)
            return new DescriptionTooLong();

        Description = description;
        return UnitResult.Success<DomainError>();
    }
    
    private Lead(Guid id, string name, string description, DealAmount amount, DateTime createAt, DateTime updateAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Amount = amount;
        CreatedAt = createAt;
        UpdatedAt = updateAt;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Lead other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}

