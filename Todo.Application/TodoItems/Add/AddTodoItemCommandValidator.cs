namespace Todo.Application.TodoItems.Add;

internal class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand>
{
    public AddTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(TodoItemsErrorCodes.MissingTitle);
    }
}
