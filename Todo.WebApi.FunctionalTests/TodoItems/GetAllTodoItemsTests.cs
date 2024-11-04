using System.Net;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class GetAllTodoItemsTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"{TodoItemData.BaseUrl}?includecompleted=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
