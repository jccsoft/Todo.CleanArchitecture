using Todo.Application.TodoItems.Complete;

namespace Todo.WebApi.Endpoints.TodoItems;

public class CompleteTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(BaseUrls.TodoItems, async (
            Guid todoItemId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoItemCommand(todoItemId);

            Result result = await sender.Send(command, cancellationToken);

            return result;
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Complete");
    }
}
