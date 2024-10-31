namespace Todo.Application.TodoItems.Update;

internal class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(TodoItemsErrorCodes.MissingTitle);
    }
}