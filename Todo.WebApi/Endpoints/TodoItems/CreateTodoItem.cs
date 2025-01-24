using Todo.Application.TodoItems.Create;

namespace Todo.WebApi.Endpoints.TodoItems;

public class CreateTodoItem : IEndpoint
{
    public sealed record CreateTodoItemRequest(string Title);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BaseUrls.TodoItems, async (
            CreateTodoItemRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateTodoItemCommand(request.Title);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Create")
        .Produces<Guid>(StatusCodes.Status200OK);
    }
}
