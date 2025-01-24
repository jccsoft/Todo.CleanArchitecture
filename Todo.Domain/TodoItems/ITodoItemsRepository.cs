namespace Todo.Domain.TodoItems;

public interface ITodoItemsRepository
{
    Task<List<TodoItem>> GetAllAsync(bool? includeCompleted = false, CancellationToken cancellationToken = default);
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> CreateAsync(TodoItem item, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
