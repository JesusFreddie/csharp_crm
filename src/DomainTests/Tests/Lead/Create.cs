using Domain.Entity;
using Domain.Error.Lead;
using FluentAssertions;

namespace DomainTests.Tests.Lead;

public class Create
{
    [Fact]
    public void Create_Lead()
    {
        var id = Guid.NewGuid();
        const string name = "TestLeadName";
        const string description = "TestLeadDescription";
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = Domain.Entity.Lead.Create(id, name, description, DealAmount.Empty(), createdAt, updatedAt);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
        result.Value.Id.Should().Be(id);
        result.Value.Description.Should().Be(description);
        result.Value.CreatedAt.Should().Be(createdAt);
        result.Value.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void Failed_Create_Lead_Too_Long_Name()
    {
        var id = Guid.NewGuid();
        var name = new string('a', 200);
        const string description = "TestLeadDescription";
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = Domain.Entity.Lead.Create(id, name, description, DealAmount.Empty(), createdAt, updatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NameTooLong>();
    }

    [Fact]
    public void Failed_Create_Lead_Empty_Name()
    {
        var id = Guid.NewGuid();
        var name = string.Empty;
        const string description = "TestLeadDescription";

        var result = Domain.Entity.Lead.Create(id, name, description, DealAmount.Empty(), DateTime.UtcNow, DateTime.UtcNow);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<NameRequired>();
    }
}