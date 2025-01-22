using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Todo.Application.Abstractions.Data;

namespace Todo.Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        TodoItemsRepository = _scope.ServiceProvider.GetRequiredService<ITodoItemsRepository>();
        UnitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        Faker = new Faker();
    }

    protected ISender Sender { get; }

    protected ITodoItemsRepository TodoItemsRepository { get; }
    protected IUnitOfWork UnitOfWork { get; }

    protected Faker Faker { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope.Dispose();
        }
    }
}
