using API.DI;
using API.Settings;
using FastEndpoints;
using FastEndpoints.Swagger;
using ErrorResponse = API.Endpoints.ErrorResponse;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitDbContext(builder.Configuration);

builder.Services.InitEndpoints();
builder.Services.InitSwagger();

builder.Services.AddLeadServices();

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
