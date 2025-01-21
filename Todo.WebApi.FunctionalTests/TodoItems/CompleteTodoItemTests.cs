using System.Net;
using System.Net.Http.Json;
using Todo.Domain.TodoItems;
using Todo.WebApi.Endpoints.TodoItems.Complete;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class CompleteTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private readonly string _baseUrl = $"{TodoItemData.BaseUrl}/complete";

    [Fact]
    public async Task Should_ReturnNoContent_WhenRequestIsValid()
    {
        // Arrange
        var request = new CompleteTodoItemRequest(TodoItemData.Sample1Id);

        // Act
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenTodoItemDoesntExist()
    {
        // Arrange
        var request = new CompleteTodoItemRequest(Guid.NewGuid());

        // Act
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsErrors.NotFound.Code;
        problemDetails.Detail = TodoItemsErrors.NotFound.Description;
    }



    [Fact]
    public async Task Should_ReturnBadRequest_WhenTodoItemIsAlreadyComplete()
    {
        // Arrange
        var request = new CompleteTodoItemRequest(TodoItemData.Sample2Id);

        // Act
        _ = await HttpClient.PatchAsJsonAsync(_baseUrl, request);
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsErrors.NotFound.Code;
        problemDetails.Detail = TodoItemsErrors.NotFound.Description;
    }

}
