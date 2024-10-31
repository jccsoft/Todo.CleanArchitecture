using Todo.Application.TodoItems.Delete;

namespace Todo.WebApi.Endpoints.TodoItems;

public class DeleteTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(BaseUrls.TodoItems, async (
            Guid todoItemId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTodoItemCommand(todoItemId);

            Result result = await sender.Send(command, cancellationToken);

            return result;
        })
        .WithTags(Tags.TodoItems);
    }
}
