using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using Testcontainers.Redis;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Setup;

namespace Todo.Application.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("tododb")
        .WithEnvironment("MYSQL_ROOT_PASSWORD", "my-secret-pw")
        .WithResourceMapping(new FileInfo("./init-db.sql"), "/docker-entrypoint-initdb.d/")
        .Build();

    private RedisContainer? _redisContainer;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(_dbContainer.GetConnectionString()));

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
        await _dbContainer.StartAsync();

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
        await _dbContainer.StopAsync();

        if (_redisContainer is not null)
            await _redisContainer.StopAsync();
    }
}
