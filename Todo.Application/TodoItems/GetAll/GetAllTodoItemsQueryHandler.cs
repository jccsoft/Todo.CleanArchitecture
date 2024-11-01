namespace Todo.Application.TodoItems.GetAll;

internal class GetAllTodoItemsQueryHandler(
    ITodoItemsRepository todoItemsRepository)
    : IQueryHandler<GetAllTodoItemsQuery, List<TodoItemResponse>>
{
    public async Task<Result<List<TodoItemResponse>>> Handle(GetAllTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await todoItemsRepository.GetAllAsync(request.IncludeCompleted, cancellationToken);

        var output = todoItems.Select(item => new TodoItemResponse(
            item.Id,
            item.Title!.Value,
            item.IsCompleted,
            item.CreatedOnUtc,
            item.CompletedOnUtc)).ToList();

        return output;
    }
}