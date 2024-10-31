namespace Todo.Application.TodoItems.Add;

internal sealed class AddTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AddTodoItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var title = new TodoItemTitle(request.Title);
        var todoItem = TodoItem.Create(title, dateTimeProvider.UtcNow);

        int affectedRows = await todoItemsRepository.AddAsync(todoItem, cancellationToken);
        if (affectedRows == 0) return Result.Failure<Guid>(TodoItemsDomainErrors.NoRowsAffected);

        await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);

        return todoItem.Id;
    }
}
