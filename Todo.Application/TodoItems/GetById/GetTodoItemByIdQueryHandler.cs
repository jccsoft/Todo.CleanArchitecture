namespace Todo.Application.TodoItems.GetById;

internal class GetTodoItemByIdQueryHandler(
    ITodoItemsRepository todoItemsRepository)
    : IQueryHandler<GetTodoItemByIdQuery, TodoItemResponse>
{
    public async Task<Result<TodoItemResponse>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null) return Result.Failure<TodoItemResponse>(TodoItemsDomainErrors.NotFound);

        var output = new TodoItemResponse(
            todoItem.Id,
            todoItem.Title!.Value,
            todoItem.IsCompleted,
            todoItem.CreatedOnUtc,
            todoItem.CompletedOnUtc);

        return output;
    }
}
