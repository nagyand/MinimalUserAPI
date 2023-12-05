using MinimalUserAPI;
using MinimalUserAPI.Application.Extensions;
using MinimalUserAPI.Extensions;
using MinimalUserAPI.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration
        .WriteTo.File("logs/log.txt")
        .WriteTo.Console();
});

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfiguraMongoDbHealthCheck(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapGroup("api/v1/users")
   .MapUserApiV1()
   .WithTags("User API");

app.Run();

public partial class Program { }