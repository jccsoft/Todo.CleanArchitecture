namespace Todo.Application.TodoItems.Complete;

internal sealed class CompleteTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<CompleteTodoItemCommand>
{
    public async Task<Result> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null) return Result.Failure(TodoItemsDomainErrors.NotFound);

        todoItem.Complete(dateTimeProvider.UtcNow);

        int affectedRows = await todoItemsRepository.UpdateAsync(todoItem, cancellationToken);
        if (affectedRows == 0) return Result.Failure(TodoItemsDomainErrors.NoRowsAffected);

        await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);

        return Result.Success();
    }
}