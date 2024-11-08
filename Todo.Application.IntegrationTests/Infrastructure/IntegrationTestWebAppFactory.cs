using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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

            string connectionString = _mySqlContainer is not null ?
                _mySqlContainer.GetConnectionString() :
                _postgresContainer!.GetConnectionString();

            services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

            //var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(AppDbContext));
            //if (context != null)
            //{
            //    services.Remove(context);
            //    var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
            //      || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
            //    foreach (var option in options)
            //    {
            //        services.Remove(option);
            //    }
            //}

            ////services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            //services.AddDbContext<AppDbContext>(optionsBuilder =>
            //{
            //    if (Config.DatabaseType.IsMySql())
            //        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            //    else
            //        optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            //});
            //var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            //.UseInMemoryDatabase("<name>");
            //services.AddScoped<DbContextOptions<MyDbContext>>(_ => contextOptions.Options);


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
        if (Config.DatabaseType.IsMySql())
        {
            _mySqlContainer = new MySqlBuilder()
                .WithImage("mysql:8.0")
                .WithDatabase("tododb")
                .WithEnvironment("MYSQL_ROOT_PASSWORD", "my-secret-pw")
                .WithResourceMapping(new FileInfo("./init-mysql.sql"), "/docker-entrypoint-initdb.d/")
                .Build();

            await _mySqlContainer.StartAsync();
        }

        if (Config.DatabaseType.IsPostgres())
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:13.16")
                .WithDatabase("tododb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithResourceMapping(new FileInfo("./init-postgres.sql"), "/docker-entrypoint-initdb.d/")
                .Build();

            await _postgresContainer.StartAsync();
        }

        if (Config.CacheType.IsRedis())
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
