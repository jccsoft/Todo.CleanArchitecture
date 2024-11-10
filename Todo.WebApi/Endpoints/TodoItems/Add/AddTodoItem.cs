using Todo.Application.TodoItems.Add;

namespace Todo.WebApi.Endpoints.TodoItems.Add;

public class AddTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BaseUrls.TodoItems, async (
            AddTodoItemRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new AddTodoItemCommand(request.Title);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Add")
        .Produces<Guid>(StatusCodes.Status200OK);
    }
}
