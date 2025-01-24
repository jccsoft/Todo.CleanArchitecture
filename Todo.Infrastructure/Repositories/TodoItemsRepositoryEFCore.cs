using Microsoft.Extensions.Logging;
using System.Data;
using Todo.Infrastructure.Database.EFCore;

namespace Todo.Infrastructure.Repositories;

internal sealed class TodoItemsRepositoryEFCore(AppDbContext dbContext, ILogger<TodoItemsRepositoryEFCore> logger) : ITodoItemsRepository
{
    public async Task<List<TodoItem>> GetAllAsync(bool? includeCompleted = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoItems = await dbContext.TodoItems
            .AsNoTracking()
            .Where(t => t.CompletedOnUtc == null || (includeCompleted ?? false))
            .ToListAsync(cancellationToken);

            return todoItems;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(GetAllAsync));
            return [];
        }
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoItem = await dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            return todoItem;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(GetByIdAsync));
            return null;
        }
    }


    public Task<int> CreateAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.TodoItems.Add(item);

            return Task.FromResult(1);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(CreateAsync));
            return Task.FromResult(0);
        }
    }

    public Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.TodoItems.Update(item);

            return Task.FromResult(1);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(UpdateAsync));
            return Task.FromResult(0);
        }
    }

    public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoItem = await dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (todoItem is null) return 0;

            dbContext.TodoItems.Remove(todoItem);

            return 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(DeleteAsync));

            return 0;
        }
    }
}
