using System.Net;
using System.Net.Http.Json;
using Todo.Domain.TodoItems;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;
using static Todo.WebApi.Endpoints.TodoItems.CreateTodoItem;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class CreateTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateTodoItemRequest("Test Title");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(TodoItemData.BaseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsMissing()
    {
        // Arrange
        var request = new CreateTodoItemRequest("");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(TodoItemData.BaseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsErrors.MissingTitle.Code;
        problemDetails.Detail = TodoItemsErrors.MissingTitle.Description;
    }

}
