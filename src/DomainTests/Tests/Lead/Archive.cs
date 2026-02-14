using Domain.Error.Lead;
using FluentAssertions;

namespace DomainTests.Tests.Lead;

public class Archive
{
    [Fact]
    public void Success_Archive_Lead()
    {
        var id = Guid.NewGuid();
        var name = "test";
        const string description = "TestLeadDescription";
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var resultLead = Domain.Entity.Lead.Create(id, name, description, createdAt, updatedAt);
        
        var resultArchive = resultLead.Value.Archive();

        resultArchive.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void Failed_Archive_Lead_Because_Lead_Be_Archived()
    {
        var id = Guid.NewGuid();
        var name = "test";
        const string description = "TestLeadDescription";
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var resultLead = Domain.Entity.Lead.Create(id, name, description, createdAt, updatedAt);
        resultLead.Value.Archive();
        
        var resultArchive = resultLead.Value.Archive();

        resultArchive.IsFailure.Should().BeTrue();
        resultArchive.Error.Should().BeOfType<AlreadyArchived>();
    }

    [Fact]
    public void Failed_Restore_Archive_Lead_Because_Lead_Be_Restored()
    {
        var id = Guid.NewGuid();
        var name = "test";
        const string description = "TestLeadDescription";
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var resultLead = Domain.Entity.Lead.Create(id, name, description, createdAt, updatedAt);
        
        var resultArchive = resultLead.Value.Restore();

        resultArchive.IsFailure.Should().BeTrue();
        resultArchive.Error.Should().BeOfType<NotArchived>();
    }
}