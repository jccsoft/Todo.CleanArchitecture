using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Todo.Application.Abstractions.Caching;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Caching;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Database.Dapper;
using Todo.Infrastructure.Database.EFCore;
using Todo.Infrastructure.Outbox;
using Todo.Infrastructure.Repositories;
using Todo.Infrastructure.Setup;
using Todo.Infrastructure.Time;

namespace Todo.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       string dbConnectionString,
                                                       string cacheConnectionString,
                                                       IConfigurationSection outboxConfigSection)
    {
        services
            .AddMyServices(dbConnectionString)
            .AddMyCaching(cacheConnectionString)
            .AddMyHealthChecks(dbConnectionString, cacheConnectionString)
            .AddMyBackgroundJobs(outboxConfigSection);

        return services;
    }


    private static IServiceCollection AddMyServices(this IServiceCollection services, string dbConnectionString)
    {
        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(dbConnectionString));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        if (Config.IsOrmEFCore)
        {
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            services.AddDbContext<AppDbContext>(optionsBuilder =>
            {
                if (Config.IsDbMySQL)
                    optionsBuilder.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)).UseSnakeCaseNamingConvention();
                else
                    optionsBuilder.UseNpgsql(dbConnectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<ITodoItemsRepository, TodoItemsRepositoryEFCore>();
        }
        else
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITodoItemsRepository, TodoItemsRepositoryDapper>();

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new TodoItemTitleTypeHandler());
        }


        return services;
    }


    private static IServiceCollection AddMyCaching(this IServiceCollection services, string cacheConnectionString = default)
    {
        if (Config.IsCacheInMemory)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
        }
        else if (Config.IsCacheRedis && string.IsNullOrEmpty(cacheConnectionString) == false)
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = cacheConnectionString);
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        return services;
    }


    private static IServiceCollection AddMyHealthChecks(this IServiceCollection services,
                                                      string dbConnectionString,
                                                      string cacheConnectionString)
    {
        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        if (Config.IsDbMySQL)
            healthChecksBuilder.AddMySql(dbConnectionString);
        else
            healthChecksBuilder.AddNpgSql(dbConnectionString);

        if (string.IsNullOrEmpty(cacheConnectionString) == false)
            healthChecksBuilder.AddRedis(cacheConnectionString);


        return services;
    }


    private static IServiceCollection AddMyBackgroundJobs(this IServiceCollection services,
                                                        IConfigurationSection outboxConfigSection)
    {
        if (outboxConfigSection.Exists())
        {
            services.Configure<OutboxOptions>(outboxConfigSection);

            services.AddQuartz();

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
        }

        return services;
    }
}
