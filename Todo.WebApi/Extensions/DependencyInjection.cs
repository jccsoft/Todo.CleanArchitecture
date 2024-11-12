using Todo.WebApi.OpenApi;

namespace Todo.WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddMyApiVersioning();

        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        return services;
    }
}
