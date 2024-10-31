using Todo.Application.TodoItems.GetById;

namespace Todo.WebApi.Endpoints.TodoItems;

public class GetByIdTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseUrls.TodoItems}/{"todoItemId"}", async (
            Guid todoItemId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodoItemByIdQuery(todoItemId);

            Result<TodoItemResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems);
    }
}
