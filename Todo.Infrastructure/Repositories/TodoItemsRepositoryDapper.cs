using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using Todo.Application.Abstractions.Data;

namespace Todo.Infrastructure.Repositories;

internal sealed class TodoItemsRepositoryDapper(IDbConnectionFactory dbConnectionFactory, ILogger<TodoItemsRepositoryDapper> logger) : ITodoItemsRepository
{
    private readonly IDbConnection _dbConnection = dbConnectionFactory.GetOpenConnection();

    public async Task<List<TodoItem>> GetAllAsync(bool? includeCompleted = false, CancellationToken cancellationToken = default)
    {
        try
        {
            string sql = """
                SELECT 
                    id as Id, 
                    title as Title, 
                    created_on_utc as CreatedOnUtc, 
                    completed_on_utc as CompletedOnUtc
                FROM todoitems
            """;

            if (includeCompleted == false) sql += " WHERE completed_on_utc is null";

            var todoItems = await _dbConnection.QueryAsync<TodoItem>(sql);

            return todoItems.ToList();
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
            const string sql = """
                SELECT 
                    id as Id, 
                    title as Title, 
                    created_on_utc as CreatedOnUtc, 
                    completed_on_utc as CompletedOnUtc
                FROM todoitems
                WHERE id = @Id
            """;

            var todo = await _dbConnection.QueryFirstOrDefaultAsync<TodoItem>(
                sql,
                new
                {
                    Id = id
                });

            return todo;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(GetByIdAsync));
            return null;
        }
    }


    public async Task<int> CreateAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sql = """
                INSERT INTO todoitems (id, title, created_on_utc)
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
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(CreateAsync));
            return 0;
        }
    }

    public async Task<int> UpdateAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sql = """
                UPDATE todoitems
                SET 
                    title = @Title,
                    completed_on_utc = @CompletedOnUtc
                WHERE id = @Id
            """;

            int affectedRows = await _dbConnection.ExecuteAsync(
                sql,
                new
                {
                    Title = item.Title!.Value,
                    item.CompletedOnUtc,
                    item.Id,
                },
                commandType: CommandType.Text);

            return affectedRows;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(UpdateAsync));
            return 0;
        }
    }

    public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sql = """
                DELETE FROM todoitems
                WHERE id = @Id
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
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(DeleteAsync));
            return 0;
        }
    }
}
