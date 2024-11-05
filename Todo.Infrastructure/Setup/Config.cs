namespace Todo.Infrastructure.Setup;

internal static class Config
{
    public static CacheTypes CacheType { get; } = CacheTypes.InMemory;
}
