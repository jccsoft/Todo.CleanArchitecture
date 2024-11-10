using Todo.Application.TodoItems.GetById;

namespace Todo.WebApi.Endpoints.TodoItems.GetById;

public class GetByIdTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseUrls.TodoItems}/{{id}}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodoItemByIdQuery(id);

            Result<TodoItemResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Get by Id")
        .Produces<TodoItemResponse>(StatusCodes.Status200OK);
    }
}
