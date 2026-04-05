using Application.Ports.DbContext;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace API.Settings;

public static class PersistenceExtensions
{
    public static IServiceCollection InitDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CrmDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No connection string configured");
            }

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ICrmContext, CrmDbContext>();
        
        return services;
    }
}