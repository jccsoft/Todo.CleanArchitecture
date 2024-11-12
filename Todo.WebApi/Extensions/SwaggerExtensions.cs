using Asp.Versioning.ApiExplorer;

namespace Todo.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseMySwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            foreach (ApiVersionDescription description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }

            options.DefaultModelsExpandDepth(0); // schemas section collapsed
        });

        return app;
    }
}
