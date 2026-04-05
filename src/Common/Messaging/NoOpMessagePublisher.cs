using Application.Ports.Messaging;

namespace Common.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class
    {
        // Stub implementation - does nothing
        // Replace with actual message broker implementation (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
