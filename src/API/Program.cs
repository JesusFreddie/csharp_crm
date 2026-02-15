using API.DI;
using API.Settings;
using Application;
using Domain.Repository;
using FastEndpoints;
using FastEndpoints.Swagger;
using Persistance;
using Persistance.Repository;
using ErrorResponse = API.Endpoints.ErrorResponse;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitEndpoints();
builder.Services.InitSwagger();

builder.Services.AddScoped<ILeadRepository, LeadRepository>();

builder.Services.AddLeadServices();

builder.Services.AddScoped<IUnitToWork>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Using connection string: {connectionString}");
    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("No connection string");
    
    return new UnitToWork(connectionString ?? "");
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseFastEndpoints(c =>
{
    c.Errors.ResponseBuilder = (failure, ctx, status) =>
    {
        var f = failure.First();
        return new ErrorResponse(
            f.ErrorCode,
            f.ErrorMessage);
    };
});

app.Run();
