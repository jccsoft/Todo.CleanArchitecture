namespace Todo.Infrastructure.Setup;

public static class Config
{
    public static CacheTypes CacheType { get; } = CacheTypes.InMemory;
    public static DatabaseTypes DatabaseType { get; } = DatabaseTypes.MySql;
    public static ORMTypes ORMType { get; } = ORMTypes.Dapper;


    public static bool IsMySql(this DatabaseTypes type) => type == DatabaseTypes.MySql;
    public static bool IsPostgres(this DatabaseTypes type) => type == DatabaseTypes.Postgres;

    public static bool IsDapper(this ORMTypes type) => type == ORMTypes.Dapper;
    public static bool IsEFCore(this ORMTypes type) => type == ORMTypes.EFCore;

    public static bool IsInMemory(this CacheTypes type) => type == CacheTypes.InMemory;
    public static bool IsRedis(this CacheTypes type) => type == CacheTypes.Redis;

}
