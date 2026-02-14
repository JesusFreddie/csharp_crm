using CSharpFunctionalExtensions;
using Domain.Error;
using Domain.Error.Lead;

namespace Domain.Entity;

public class Account : BaseEntity, IArchivable
{
    public string Name { get; set; }

    private Account(string name)
    {
        Name = name;
    }

    public bool IsArchived { get; private set; } = false;
    
    public UnitResult<DomainError> Archive()
    {
        if (IsArchived) return new AlreadyArchived();
        
        IsArchived = true;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Restore()
    {
        if (!IsArchived) return new NotArchived();
        
        IsArchived = false;
        return UnitResult.Success<DomainError>();
    }
}