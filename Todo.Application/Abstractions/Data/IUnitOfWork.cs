namespace Todo.Application.Abstractions.Data;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(List<Entity>? modifiedEntities = default, CancellationToken cancellationToken = default);
}
