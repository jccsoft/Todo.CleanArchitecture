﻿using Todo.Application.TodoItems.GetAll;

namespace Todo.WebApi.Endpoints.TodoItems;

public class GetAllTodoItems() : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BaseUrls.TodoItems, async (
            bool includeCompleted,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllTodoItemsQuery(includeCompleted);

            Result<List<TodoItemResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TodoItems)
        .WithSummary("Get All");
    }
}