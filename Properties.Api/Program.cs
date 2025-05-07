using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Properties.Api.Extensions;
using Properties.Api.Middleware;
using Properties.Application;
using Properties.Infraestructure;

using Serilog;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("healthCheck", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.Run();
