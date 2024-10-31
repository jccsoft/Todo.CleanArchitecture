using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Todo.Application.Abstractions.Caching;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure;
using Todo.Infrastructure.Caching;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Outbox;
using Todo.Infrastructure.Repositories;
using Todo.Infrastructure.Time;

namespace Todo.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IConfiguration configuration,
                                                       string databaseConfigKey,
                                                       string cacheConfigKey,
                                                       string outboxConfigKey)
    {
        services
            .AddPersistence(configuration, databaseConfigKey)
            .AddCaching(configuration, cacheConfigKey)
            .AddHealthChecks(configuration, databaseConfigKey, cacheConfigKey)
            .AddBackgroundJobs(configuration, outboxConfigKey);

        return services;
    }


    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string databaseConfigKey)
    {
        var connectionString = configuration.GetConnectionString(databaseConfigKey) ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TodoItemTitleTypeHandler());

        return services;
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration, string cacheConfigKey)
    {
        var connectionString = configuration.GetConnectionString(cacheConfigKey);
        if (string.IsNullOrEmpty(connectionString) == false)
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

            services.AddSingleton<ICacheService, CacheService>();
        }

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services,
                                                      IConfiguration configuration,
                                                      string databaseConfigKey,
                                                      string cacheConfigKey)
    {
        var databaseConnectionString = configuration.GetConnectionString(databaseConfigKey) ??
                               throw new ArgumentNullException(nameof(configuration));

        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        healthChecksBuilder.AddMySql(databaseConnectionString);

        var cacheConnectionString = configuration.GetConnectionString(cacheConfigKey);
        if (string.IsNullOrEmpty(cacheConnectionString) == false)
            healthChecksBuilder.AddRedis(cacheConnectionString);


        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration, string outboxConfigKey)
    {
        if (configuration.GetSection(outboxConfigKey).Exists())
        {
            services.AddScoped<IOutboxRepository, OutboxRepository>();

            services.Configure<OutboxOptions>(configuration.GetSection(outboxConfigKey));

            services.AddQuartz();

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
        }

        return services;
    }
}
