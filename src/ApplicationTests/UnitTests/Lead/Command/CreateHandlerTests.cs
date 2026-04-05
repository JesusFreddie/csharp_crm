using Application.CQRS.Lead.Command.Create;
using Application.Ports.Id;
using Application.Ports.Messaging;
using Application.Ports.DbContext;
using Application.Ports.Time;
using Contracts.Messages.Lead;
using Domain.Entity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ApplicationTests.UnitTests.Lead.Command;

public class CreateHandlerTests
{
    private readonly ICrmContext _context;
    private readonly IClock _clock;
    private readonly IIdGenerator _idGenerator;
    private readonly IMessagePublisher _messagePublisher;
    private readonly Handler _handler;

    public CreateHandlerTests()
    {
        _context = Substitute.For<ICrmContext>();
        _clock = Substitute.For<IClock>();
        _idGenerator = Substitute.For<IIdGenerator>();
        _messagePublisher = Substitute.For<IMessagePublisher>();

        _handler = new Handler(_context, _clock, _idGenerator, _messagePublisher);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessAndPublishesEvent()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var command = new Application.CQRS.Lead.Command.Create.Command("Test Lead", "Test Description");

        _idGenerator.New().Returns(leadId);
        _clock.UtcNow().Returns(now);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        await _context.Leads.Received(1).AddAsync(
            Arg.Is<Domain.Entity.Lead>(l =>
                l.Id == leadId &&
                l.Name == "Test Lead" &&
                l.Description == "Test Description"),
            Arg.Any<CancellationToken>());

        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<LeadCreatedEvent>(e =>
                e.LeadId == leadId &&
                e.Name == "Test Lead" &&
                e.Description == "Test Description" &&
                e.Amount == 0m &&
                e.PricingMode == "Manual" &&
                e.CreatedAt == now),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptyName_ReturnsFailure()
    {
        // Arrange
        var command = new Application.CQRS.Lead.Command.Create.Command("", "Test Description");
        var now = DateTime.UtcNow;

        _idGenerator.New().Returns(Guid.NewGuid());
        _clock.UtcNow().Returns(now);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("lead.name.required");

        await _context.Leads.DidNotReceive().AddAsync(Arg.Any<Domain.Entity.Lead>(), Arg.Any<CancellationToken>());
        await _context.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _messagePublisher.DidNotReceive().PublishAsync(Arg.Any<LeadCreatedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NameTooLong_ReturnsFailure()
    {
        // Arrange
        var longName = new string('a', 200);
        var command = new Application.CQRS.Lead.Command.Create.Command(longName, "Test Description");
        var now = DateTime.UtcNow;

        _idGenerator.New().Returns(Guid.NewGuid());
        _clock.UtcNow().Returns(now);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("lead.name.too_long");

        await _context.Leads.DidNotReceive().AddAsync(Arg.Any<Domain.Entity.Lead>(), Arg.Any<CancellationToken>());
        await _context.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _messagePublisher.DidNotReceive().PublishAsync(Arg.Any<LeadCreatedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsMethodsInCorrectOrder()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var command = new Application.CQRS.Lead.Command.Create.Command("Test Lead", "Test Description");
        var callOrder = new List<string>();

        _idGenerator.New().Returns(leadId);
        _clock.UtcNow().Returns(now);

        _context.Leads.AddAsync(Arg.Any<Domain.Entity.Lead>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                callOrder.Add("AddAsync");
                return default(ValueTask<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Domain.Entity.Lead>>);
            });

        _context.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                callOrder.Add("SaveChangesAsync");
                return Task.FromResult(1);
            });

        _messagePublisher.PublishAsync(Arg.Any<LeadCreatedEvent>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                callOrder.Add("PublishAsync");
                return Task.CompletedTask;
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        callOrder.Should().ContainInOrder("AddAsync", "SaveChangesAsync", "PublishAsync");
    }

    [Fact]
    public async Task Handle_ValidCommand_WithWhitespaceInName_TrimsName()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var command = new Application.CQRS.Lead.Command.Create.Command("  Test Lead  ", "  Test Description  ");

        _idGenerator.New().Returns(leadId);
        _clock.UtcNow().Returns(now);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        // Verify the result is not null and matches expected values
        var expected = new Application.Entity.Lead(Guid.Empty, "Test Lead", "Test Description", DateTime.MinValue, DateTime.MinValue);
        result.Value.Should().NotBeNull();
        // Since it's a positional record, we can only verify by creating expected instance
        // The actual verification happens in the mock assertions below

        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<LeadCreatedEvent>(e =>
                e.Name == "Test Lead" &&
                e.Description == "Test Description"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_UsesEmptyDealAmount()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var command = new Application.CQRS.Lead.Command.Create.Command("Test Lead", "Test Description");

        _idGenerator.New().Returns(leadId);
        _clock.UtcNow().Returns(now);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<LeadCreatedEvent>(e =>
                e.Amount == 0m &&
                e.PricingMode == "Manual"),
            Arg.Any<CancellationToken>());
    }
}
