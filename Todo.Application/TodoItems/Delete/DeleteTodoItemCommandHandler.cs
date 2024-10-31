namespace Todo.Application.TodoItems.Delete;

internal sealed class DeleteTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteTodoItemCommand>
{
    public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        int affectedRows = await todoItemsRepository.DeleteAsync(request.Id, cancellationToken);
        if (affectedRows == 0) return Result.Failure(TodoItemsDomainErrors.NoRowsAffected);

        await unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success();
    }
}
