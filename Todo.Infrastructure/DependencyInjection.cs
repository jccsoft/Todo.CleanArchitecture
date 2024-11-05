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
using Todo.Infrastructure.Setup;
using Todo.Infrastructure.Time;

namespace Todo.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IConfiguration configuration,
                                                       string databaseConfigKey,
                                                       string? cacheConfigKey = default,
                                                       string? outboxConfigKey = default)
    {
        services.AddPersistence(configuration, databaseConfigKey);
        services.AddCaching(configuration, cacheConfigKey);
        services.AddHealthChecks(configuration, databaseConfigKey, cacheConfigKey);
        services.AddBackgroundJobs(configuration, outboxConfigKey);

        return services;
    }


    public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                    IConfiguration configuration,
                                                    string databaseConfigKey)
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


    private static IServiceCollection AddCaching(this IServiceCollection services,
                                                 IConfiguration configuration,
                                                 string? cacheConfigKey)
    {
        if (Config.CacheType == CacheTypes.InMemory)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
        }
        else if (Config.CacheType == CacheTypes.Redis && string.IsNullOrEmpty(cacheConfigKey) == false)
        {
            var connectionString = configuration.GetConnectionString(cacheConfigKey);
            if (string.IsNullOrEmpty(connectionString) == false)
            {
                services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

                services.AddSingleton<ICacheService, RedisCacheService>();
            }
        }

        return services;
    }


    private static IServiceCollection AddHealthChecks(this IServiceCollection services,
                                                      IConfiguration configuration,
                                                      string? databaseConfigKey = default,
                                                      string? cacheConfigKey = default)
    {
        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        if (string.IsNullOrEmpty(databaseConfigKey) == false)
        {
            var databaseConnectionString = configuration.GetConnectionString(databaseConfigKey) ??
                               throw new ArgumentNullException(nameof(configuration));

            healthChecksBuilder.AddMySql(databaseConnectionString);
        }

        if (string.IsNullOrEmpty(cacheConfigKey) == false)
        {
            var cacheConnectionString = configuration.GetConnectionString(cacheConfigKey);
            if (string.IsNullOrEmpty(cacheConnectionString) == false)
                healthChecksBuilder.AddRedis(cacheConnectionString);
        }

        return services;
    }


    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services,
                                                        IConfiguration configuration,
                                                        string? outboxConfigKey = default)
    {
        if (string.IsNullOrEmpty(outboxConfigKey) == false && configuration.GetSection(outboxConfigKey).Exists())
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
