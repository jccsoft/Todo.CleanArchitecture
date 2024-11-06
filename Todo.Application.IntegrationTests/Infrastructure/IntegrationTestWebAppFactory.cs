using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Setup;

namespace Todo.Application.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MySqlContainer? _mySqlContainer;
    private PostgreSqlContainer? _postgresContainer;
    private RedisContainer? _redisContainer;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));

            if (_mySqlContainer is not null)
                services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(_mySqlContainer.GetConnectionString()));

            if (_postgresContainer is not null)
                services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(_postgresContainer.GetConnectionString()));


            if (_redisContainer is not null)
            {
                services.RemoveAll(typeof(RedisCacheOptions));
                services.AddStackExchangeRedisCache(redisCacheOptions =>
                    redisCacheOptions.Configuration = _redisContainer.GetConnectionString());
            }
        });
    }

    public async Task InitializeAsync()
    {
        if (Config.DatabaseType == DatabaseTypes.MySql)
        {
            _mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithDatabase("tododb")
            .WithEnvironment("MYSQL_ROOT_PASSWORD", "my-secret-pw")
            .WithResourceMapping(new FileInfo("./init-mysql.sql"), "/docker-entrypoint-initdb.d/")
            .Build();

            await _mySqlContainer.StartAsync();
        }
        else
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("runtrackr")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithResourceMapping(new FileInfo("./init-postgres.sql"), "/docker-entrypoint-initdb.d/")
                .Build();

            await _postgresContainer.StartAsync();
        }

        if (Config.CacheType == CacheTypes.Redis)
        {
            _redisContainer = new RedisBuilder()
                .WithImage("redis:latest")
                .Build();

            await _redisContainer.StartAsync();
        }
    }

    public new async Task DisposeAsync()
    {
        if (_mySqlContainer is not null)
            await _mySqlContainer.StopAsync();

        if (_postgresContainer is not null)
            await _postgresContainer.StopAsync();

        if (_redisContainer is not null)
            await _redisContainer.StopAsync();
    }
}
