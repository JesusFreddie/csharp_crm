using Domain.Entity;
using Domain.Error.Lead;
using FluentAssertions;

namespace DomainTests.Tests.Lead;

public class SetName
{
    [Fact]
    public void Update_Name_Is_Success()
    {
        var now = DateTime.UtcNow;
        var leadResult = Domain.Entity.Lead.Create(Guid.NewGuid(), "OldName", "", DealAmount.Empty(), now, now);
        var newName = "NewName";

        var result = leadResult.Value.SetName(newName);

        result.IsSuccess.Should().BeTrue();
        leadResult.Value.Name.Should().Be(newName);
    }

    [Fact]
    public void Update_Name_Is_Failure_Name_Too_Long()
    {
        var now = DateTime.UtcNow;
        var leadResult = Domain.Entity.Lead.Create(Guid.NewGuid(), "OldName", "", DealAmount.Empty(), now, now);
        var newName = new string('a', 1000);

        var result = leadResult.Value.SetName(newName);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<NameTooLong>();
    }

    [Fact]
    public void Update_Name_Is_Failure_Archived()
    {
        var now = DateTime.UtcNow;
        var leadResult = Domain.Entity.Lead.Create(Guid.NewGuid(), "OldName", "", DealAmount.Empty(), now, now);
        leadResult.Value.Archive();

        var newName = "NewName";
        var result = leadResult.Value.SetName(newName);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<CannotModifyArchived>();
    }

}