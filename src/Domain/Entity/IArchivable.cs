using CSharpFunctionalExtensions;
using Domain.Error;

namespace Domain.Entity;

public interface IArchivable
{
    bool IsArchived { get; }

    public UnitResult<DomainError> Archive();
    public UnitResult<DomainError> Restore();
}