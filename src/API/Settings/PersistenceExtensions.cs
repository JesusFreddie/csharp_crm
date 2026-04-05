using Application.Ports.DbContext;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace API.Settings;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        }

        services.AddDbContext<CrmDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ICrmContext, CrmDbContext>();

        return services;
    }
}
