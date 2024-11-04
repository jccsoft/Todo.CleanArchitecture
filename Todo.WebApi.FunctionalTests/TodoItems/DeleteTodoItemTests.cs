using System.Net;
using Todo.Domain.TodoItems;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class DeleteTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnNoContent_WhenRequestIsValid()
    {
        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{TodoItemData.BaseUrl}/{TodoItemData.Sample3Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenTodoItemDoesNotExist()
    {
        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{TodoItemData.BaseUrl}/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsDomainErrors.NotFound.Code;
        problemDetails.Detail = TodoItemsDomainErrors.NotFound.Description;
    }
}
