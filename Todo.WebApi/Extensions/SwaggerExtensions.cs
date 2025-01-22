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

            foreach (string groupName in descriptions.Select(description => description.GroupName))
            {
                options.SwaggerEndpoint(
                    $"/swagger/{groupName}/swagger.json",
                    groupName.ToUpperInvariant());
            }

            options.DefaultModelsExpandDepth(0); // schemas section collapsed
        });

        return app;
    }
}
