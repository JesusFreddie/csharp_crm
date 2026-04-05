using Application.Ports.Handler;
using Application.Ports.Messaging;
using Common.Messaging;

namespace API.DI;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Auto-register all CQRS handlers from Application assembly
        var applicationAssembly = typeof(ICommandHandler).Assembly;

        services.Scan(scan => scan
            .FromAssemblies(applicationAssembly)
            .AddClasses(classes => classes.AssignableTo<ICommandHandler>())
            .AsSelf()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(applicationAssembly)
            .AddClasses(classes => classes.AssignableTo<IQueryHandler>())
            .AsSelf()
            .WithScopedLifetime());

        // Messaging
        services.AddScoped<IMessagePublisher, NoOpMessagePublisher>();

        return services;
    }
}
