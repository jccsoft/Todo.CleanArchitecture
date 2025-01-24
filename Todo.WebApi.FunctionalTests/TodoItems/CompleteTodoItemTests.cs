using System.Net;
using System.Net.Http.Json;
using Todo.Domain.TodoItems;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class CompleteTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnNoContent_WhenRequestIsValid()
    {
        // Arrange
        string requestUri = $"{TodoItemData.BaseUrl}/{TodoItemData.Sample1Id}/complete";

        // Act
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(requestUri, new { });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenTodoItemDoesntExist()
    {
        // Arrange
        string requestUri = $"{TodoItemData.BaseUrl}/{Guid.NewGuid()}/complete";

        // Act
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(requestUri, new { });

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
        string requestUri = $"{TodoItemData.BaseUrl}/{TodoItemData.Sample2Id}/complete";

        // Act
        _ = await HttpClient.PatchAsJsonAsync(requestUri, new { });
        HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(requestUri, new { });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsErrors.NotFound.Code;
        problemDetails.Detail = TodoItemsErrors.NotFound.Description;
    }

}
