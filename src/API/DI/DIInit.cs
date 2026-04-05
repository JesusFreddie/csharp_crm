using Application.Ports.Messaging;
using Common.Messaging;

namespace API.DI;

public static class DiInit
{
    public static IServiceCollection AddLeadServices(this IServiceCollection services)
    {
        services.AddScoped<Application.CQRS.Lead.Command.Delete.Handler>();
        services.AddScoped<Application.CQRS.Lead.Command.Archive.Handler>();
        services.AddScoped<Application.CQRS.Lead.Command.Create.Handler>();
        services.AddScoped<Application.CQRS.Lead.Command.Restore.Handler>();
        services.AddScoped<Application.CQRS.Lead.Query.GetById.Handler>();

        services.AddScoped<IMessagePublisher, NoOpMessagePublisher>();

        return services;
    }
}