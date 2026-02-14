using FluentMigrator.Runner;
using Persistance.Migration;

Console.WriteLine("Migrator started");

var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices((ctx, services) =>
{
    var connection = ctx.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine(connection);
    services
        .AddFluentMigratorCore()
        .ConfigureRunner(rb =>
        {
            rb.AddPostgres()
                .WithGlobalConnectionString(connection)
                .ScanIn(typeof(Lead_Initial).Assembly).For.Migrations();
        });
});

var app = host.Build();

using var scope = app.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

runner.MigrateUp();