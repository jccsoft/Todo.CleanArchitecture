namespace Todo.Application.TodoItems.Complete;

internal sealed class CompleteTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CompleteTodoItemCommand>
{
    public async Task<Result> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null) return Result.Failure(TodoItemsDomainErrors.NotFound);

        var result = todoItem.Complete(dateTimeProvider.UtcNow);
        if (result.IsFailure) return Result.Failure(result.Error);

        int affectedRows = await todoItemsRepository.UpdateAsync(todoItem, cancellationToken);
        if (affectedRows == 0) return Result.Failure(TodoItemsDomainErrors.NoRowsAffected);

        int saveChangesResult = await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);
        if (saveChangesResult == 0) return Result.Failure(TodoItemsDomainErrors.NoRowsAffected);

        return Result.Success();
    }
}