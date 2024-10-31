using Todo.Application.TodoItems.GetAll;

namespace Todo.WebApi.Endpoints.TodoItems;

public class GetAllTodoItems(ILogger<GetAllTodoItems> logger) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BaseUrls.TodoItems, async (
            bool includeCompleted,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTodoItemsQuery(includeCompleted);

            Result<List<TodoItemResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems);
    }
}