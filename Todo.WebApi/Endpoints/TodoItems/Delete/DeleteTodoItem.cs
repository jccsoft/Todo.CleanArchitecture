using Todo.Application.TodoItems.Delete;

namespace Todo.WebApi.Endpoints.TodoItems.Delete;

public class DeleteTodoItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{BaseUrls.TodoItems}/{{id}}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTodoItemCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Delete");
    }
}
