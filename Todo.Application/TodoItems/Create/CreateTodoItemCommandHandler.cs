namespace Todo.Application.TodoItems.Create;

internal sealed class CreateTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateTodoItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var title = new TodoItemTitle(request.Title);
        var todoItem = TodoItem.Create(title, dateTimeProvider.UtcNow);

        int affectedRows = await todoItemsRepository.AddAsync(todoItem, cancellationToken);
        if (affectedRows == 0) return Result.Failure<Guid>(TodoItemsDomainErrors.NoRowsAffected);

        await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);

        return todoItem.Id;
    }
}
