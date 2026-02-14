using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace API.Settings;

public static class Endpoints
{
    public static IServiceCollection InitEndpoints(this IServiceCollection services)
    {
        services
            .AddFastEndpoints()
            .SwaggerDocument()
            .ConfigureHttpJsonOptions(opt =>
            {
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                opt.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            });
        return services;
    }
}