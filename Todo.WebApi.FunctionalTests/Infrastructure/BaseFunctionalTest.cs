namespace Todo.WebApi.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest(FunctionalTestWebAppFactory factory) : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; set; } = factory.CreateClient();
}
