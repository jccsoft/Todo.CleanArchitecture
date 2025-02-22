using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.AddAllLayersServices();

builder.Services.AddMyEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapMyEndpoints();

app.UseMySwaggerWithUi();

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();


await app.RunAsync();


// Required for functional and integration tests to work.
#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program;
#pragma warning restore S1118 // Utility classes should not have public constructors
