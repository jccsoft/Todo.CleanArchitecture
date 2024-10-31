namespace Todo.Application.TodoItems.Create;

internal class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(TodoItemsErrorCodes.MissingTitle);
    }
}
