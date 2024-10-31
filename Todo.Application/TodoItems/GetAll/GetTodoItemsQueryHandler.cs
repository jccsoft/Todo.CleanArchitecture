namespace Todo.Application.TodoItems.GetAll;

internal class GetTodoItemsQueryHandler(
    ITodoItemsRepository todoItemsRepository)
    : IQueryHandler<GetTodoItemsQuery, List<TodoItemResponse>>
{
    public async Task<Result<List<TodoItemResponse>>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await todoItemsRepository.GetAllAsync(request.IncludeCompleted, cancellationToken);

        if (todoItems is null) return new List<TodoItemResponse>();

        var output = todoItems.Select(item => new TodoItemResponse(
            item.Id,
            item.Title!.Value,
            item.IsCompleted,
            item.CreatedOnUtc,
            item.CompletedOnUtc)).ToList();

        return output;
    }
}