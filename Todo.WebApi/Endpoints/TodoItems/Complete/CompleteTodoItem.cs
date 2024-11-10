using Todo.Application.TodoItems.Complete;

namespace Todo.WebApi.Endpoints.TodoItems.Complete;

public class CompleteTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{BaseUrls.TodoItems}/complete", async (
            CompleteTodoItemRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoItemCommand(request.Id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Complete")
        .Produces(StatusCodes.Status204NoContent);
    }
}
