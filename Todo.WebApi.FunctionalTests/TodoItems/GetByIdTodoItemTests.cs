using System.Net;
using Todo.Domain.TodoItems;
using Todo.WebApi.FunctionalTests.Contracts;
using Todo.WebApi.FunctionalTests.Extensions;
using Todo.WebApi.FunctionalTests.Infrastructure;

namespace Todo.WebApi.FunctionalTests.TodoItems;

public class GetByIdTodoItemTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"{TodoItemData.BaseUrl}/{TodoItemData.Sample4Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenTodoItemDoesntExist()
    {
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"{TodoItemData.BaseUrl}/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        problemDetails.Title = TodoItemsDomainErrors.NotFound.Code;
        problemDetails.Detail = TodoItemsDomainErrors.NotFound.Description;
    }
}
