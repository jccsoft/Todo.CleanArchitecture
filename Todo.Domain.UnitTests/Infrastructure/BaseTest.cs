using Todo.SharedKernel;

namespace Todo.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{
    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        return entity.DomainEvents.OfType<T>().SingleOrDefault() ??
            throw new Exception($"{typeof(T).Name} was not published");
    }
}