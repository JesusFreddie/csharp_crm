using Application.Auth;
using Application.CQRS.Lead.Query.GetById;
using CSharpFunctionalExtensions;
using Shared;

namespace Application.Policy.Lead;

public interface IGetLeadPolicy
{
    public Task<UnitResult<BaseError>> CanExecute(ICurrentUser userId, Query cmd, CancellationToken ct = default);
}