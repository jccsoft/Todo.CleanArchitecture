using Dapper;
using System.Data;
using Todo.Application.Abstractions.Data;

namespace Todo.Infrastructure.Repositories;

internal sealed class TodoItemsRepository(IDbConnectionFactory dbConnectionFactory) : ITodoItemsRepository
{
    private readonly IDbConnection _dbConnection = dbConnectionFactory.GetOpenConnection();

    public async Task<List<TodoItem>> GetAllAsync(bool includeCompleted = false, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Title, IsCompleted, CreatedOnUtc, CompletedOnUtc
            FROM todoitems
            WHERE IsCompleted = 0 or IsCompleted = @IsCompleted
            """;

        var todoItems = await _dbConnection.QueryAsync<TodoItem>(
            sql,
            new
            {
                IsCompleted = includeCompleted ? 1 : 0
            });

        return todoItems.ToList();
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Title, IsCompleted, CreatedOnUtc, CompletedOnUtc
            FROM todoitems
            WHERE Id = @Id
            """;

        var todo = await _dbConnection.QueryFirstOrDefaultAsync<TodoItem>(
            sql,
            new
            {
                Id = id
            });

        return todo;
    }


    public async Task<int> AddAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO todoitems (Id, Title, CreatedOnUtc)
            VALUES (@Id, @Title, @CreatedOnUtc)
            """;

        int affectedRows = await _dbConnection.ExecuteAsync(
            sql,
            new
            {
                item.Id,
                Title = item.Title!.Value,
                item.CreatedOnUtc
            },
            commandType: CommandType.Text);

        return affectedRows;
    }

    public async Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE todoitems
            SET 
                Title = @Title,
                IsCompleted = @IsCompleted,
                CompletedOnUtc = @CompletedOnUtc
            WHERE Id = @Id
            """;

        int affectedRows = await _dbConnection.ExecuteAsync(
            sql,
            new
            {
                Title = item.Title!.Value,
                IsCompleted = item.IsCompleted ? 1 : 0,
                item.CompletedOnUtc,
                item.Id,
            },
            commandType: CommandType.Text);

        return affectedRows;
    }

    public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM todoitems
            WHERE Id = @Id
            """;

        int affectedRows = await _dbConnection.ExecuteAsync(
            sql,
            new
            {
                Id = id
            },
            commandType: CommandType.Text);

        return affectedRows;
    }
}
