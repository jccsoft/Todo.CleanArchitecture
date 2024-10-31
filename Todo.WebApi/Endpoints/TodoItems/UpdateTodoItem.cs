using Todo.Application.TodoItems.Update;

namespace Todo.WebApi.Endpoints.TodoItems;

public class UpdateTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(BaseUrls.TodoItems, async (
            UpdateTodoItemRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTodoItemCommand(request.Id, request.Title, request.IsCompleted);

            Result result = await sender.Send(command, cancellationToken);

            return result;
        })
        .WithTags(Tags.TodoItems);
    }
}
