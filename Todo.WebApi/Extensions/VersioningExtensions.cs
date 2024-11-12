using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Todo.WebApi.Extensions;

public static class VersioningExtensions
{
    public static IServiceCollection AddMyApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static RouteGroupBuilder GetVersionedGroup(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        return app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
    }
}
