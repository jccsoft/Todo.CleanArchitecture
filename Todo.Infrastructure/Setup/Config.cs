namespace Todo.Infrastructure.Setup;

public static class Config
{
    public static CacheTypes CacheType { get; } = CacheTypes.InMemory;
    public static DatabaseTypes DatabaseType { get; } = DatabaseTypes.MySql;
}
