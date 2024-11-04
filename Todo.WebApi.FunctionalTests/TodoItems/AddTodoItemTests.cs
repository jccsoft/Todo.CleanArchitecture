using System.Net;
using System.Net.Http.Json;
using Todo.Domain.TodoItems;
using Todo.WebApi.Endpoints.TodoItems.Add;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class AddTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new AddTodoItemRequest("Test Title");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(TodoItemData.BaseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsMissing()
    {
        // Arrange
        var request = new AddTodoItemRequest("");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(TodoItemData.BaseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsDomainErrors.MissingTitle.Code;
        problemDetails.Detail = TodoItemsDomainErrors.MissingTitle.Description;
    }

}
