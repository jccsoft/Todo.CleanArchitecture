namespace Todo.Application.Abstractions.Data;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(List<Entity>? entities = default, CancellationToken cancellationToken = default);
}
