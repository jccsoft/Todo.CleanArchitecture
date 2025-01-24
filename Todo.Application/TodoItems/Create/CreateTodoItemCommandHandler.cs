namespace Todo.Application.TodoItems.Create;

internal sealed class CreateTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateTodoItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var title = new TodoItemTitle(request.Title);
            var todoItem = TodoItem.Create(title, dateTimeProvider.UtcNow);

            int affectedRows = await todoItemsRepository.CreateAsync(todoItem, cancellationToken);
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
