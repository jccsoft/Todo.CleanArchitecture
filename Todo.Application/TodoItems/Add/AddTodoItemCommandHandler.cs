namespace Todo.Application.TodoItems.Add;

internal sealed class AddTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AddTodoItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var title = new TodoItemTitle(request.Title);
            var todoItem = TodoItem.Create(title, dateTimeProvider.UtcNow);

            int affectedRows = await todoItemsRepository.AddAsync(todoItem, cancellationToken);
            if (affectedRows == 0) return Result.Failure<Guid>(TodoItemsErrors.NoRowsAffected);

            int saveChangesResult = await unitOfWork.SaveChangesAsync([todoItem], cancellationToken);
            if (saveChangesResult == 0) return Result.Failure<Guid>(TodoItemsErrors.NoRowsAffected);

            return todoItem.Id;
        }
        catch (ArgumentNullException ex) when ((ex.Message ?? "").Equals(".ctor (Parameter 'title')"))
        {
            return Result.Failure<Guid>(TodoItemsErrors.MissingTitle);
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(TodoItemsErrors.UnknownError);
        }
    }
}
