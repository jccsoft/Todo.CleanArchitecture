namespace Todo.Application.TodoItems.Update;

internal sealed class UpdateTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateTodoItemCommand>
{
    public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null) return Result.Failure(TodoItemsDomainErrors.NotFound);

        var title = new TodoItemTitle(request.Title);

        if (todoItem.Title == title && todoItem.IsCompleted == request.IsCompleted)
            return Result.Success();

        todoItem.Title = title;

        if (todoItem.IsCompleted == false && request.IsCompleted)
            todoItem.MarkAsCompleted(dateTimeProvider.UtcNow);

        if (todoItem.IsCompleted && request.IsCompleted == false)
            todoItem.MarkAsNotCompleted();


        int affectedRows = await todoItemsRepository.UpdateAsync(todoItem, cancellationToken);
        if (affectedRows == 0) return Result.Failure(TodoItemsDomainErrors.NoRowsAffected);

        await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);

        return Result.Success();
    }
}
