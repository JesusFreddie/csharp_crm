using Application.Ports.Id;
using Application.Ports.Time;
using Common.Id;
using Common.Time;

namespace API.DI;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IClock, SystemClock>();
        services.AddScoped<IIdGenerator, GuidIdGenerator>();

        return services;
    }
}
