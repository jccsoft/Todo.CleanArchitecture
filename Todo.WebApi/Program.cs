using Asp.Versioning;
using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;
using Todo.Infrastructure.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

string dbConnectionString = builder.Environment.IsDevelopment() ?
    builder.Configuration.GetConnectionString(Config.IsDbMySQL ? "MySQL" : "PostgreSQL") ?? "" :
    (Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb") ?? "").Replace(":", ";port=").Replace("localdb", "tododb");

string redisConnectionString = builder.Configuration.GetConnectionString("Cache") ?? "";

var outboxConfigSection = builder.Configuration.GetSection("Outbox");

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(dbConnectionString, redisConnectionString, outboxConfigSection);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

var app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

//if (app.Environment.IsDevelopment())
app.UseSwaggerWithUi();


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

//app.MapControllers();

await app.RunAsync();


// Required for functional and integration tests to work.
public partial class Program;
