using Todo.Application.TodoItems.Complete;

namespace Todo.WebApi.Endpoints.TodoItems;

public class CompleteTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{BaseUrls.TodoItems}/{{id:guid}}/complete", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTodoItemCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Complete")
        .Produces(StatusCodes.Status204NoContent);
    }
}
