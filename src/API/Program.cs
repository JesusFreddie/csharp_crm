using API.DI;
using API.Settings;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

// Application
builder.Services.AddApplicationServices();

// API
builder.Services.AddEndpoints();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseEndpoints();

app.Run();
