using FastEndpoints.Swagger;

namespace API.Settings;

public static class Swagger
{
    public static IServiceCollection InitSwagger(this IServiceCollection services)
    {
        // services.SwaggerDocument(o =>
        // {
        //     o.DocumentSettings = s =>
        //     {
        //         s.Title = "Crm API";
        //         // s.Version = "v1";
        //     };
        // });
        return services;
    }
}