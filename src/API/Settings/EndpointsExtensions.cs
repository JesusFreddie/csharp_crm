using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace API.Settings;

public static class EndpointsExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
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

    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(config =>
        {
            config.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
            {
                var firstFailure = failures.First();
                return new Endpoints.ErrorResponse(
                    firstFailure.ErrorCode,
                    firstFailure.ErrorMessage);
            };
        });

        return app;
    }
}
