using Todo.Infrastructure.Setup;

namespace Todo.WebApi.Extensions;

public static class ProgramExtensions
{
    public static void AddAllLayersServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplication()
            .AddPresentation()
            .AddInfrastructure(builder.GetDbConnectionString(),
                               builder.GetCacheConnectionString(),
                               builder.GetOutboxConfigSection());
    }

    private static string GetDbConnectionString(this WebApplicationBuilder builder) =>
        builder.Environment.IsDevelopment() ?
        builder.Configuration.GetConnectionString(Config.DbConnectionStringName) ?? "" :
        (Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb") ?? "").Replace(":", ";port=").Replace("localdb", "tododb");

    private static string GetCacheConnectionString(this WebApplicationBuilder builder) =>
        builder.Configuration.GetConnectionString("Cache") ?? "";

    private static IConfigurationSection GetOutboxConfigSection(this WebApplicationBuilder builder) =>
        builder.Configuration.GetSection("Outbox");
}
