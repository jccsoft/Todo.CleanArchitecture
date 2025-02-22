﻿namespace Todo.Application.TodoItems.Delete;

internal sealed class DeleteTodoItemCommandHandler(
    ITodoItemsRepository todoItemsRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteTodoItemCommand>
{
    public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todoItem is null) return Result.Failure(TodoItemsErrors.NotFound);

        int affectedRows = await todoItemsRepository.DeleteAsync(request.Id, cancellationToken);
        if (affectedRows == 0) return Result.Failure(TodoItemsErrors.NoRowsAffected);

        int saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        if (saveChangesResult == 0) return Result.Failure(TodoItemsErrors.NoRowsAffected);

        return Result.Success();
    }
}
