namespace Todo.Infrastructure.Setup;

public static class Config
{
    private static readonly CacheTypes _cacheType = CacheTypes.InMemory;
    private static readonly DatabaseTypes _databaseType = DatabaseTypes.MySql;
    private static readonly ORMTypes _ormType = ORMTypes.EFCore;


    public static bool IsDbMySQL => _databaseType == DatabaseTypes.MySql;
    public static bool IsDbPostgres => _databaseType == DatabaseTypes.Postgres;

    public static bool IsOrmDapper => _ormType == ORMTypes.Dapper;
    public static bool IsOrmEFCore => _ormType == ORMTypes.EFCore;

    public static bool IsCacheInMemory => _cacheType == CacheTypes.InMemory;
    public static bool IsCacheRedis => _cacheType == CacheTypes.Redis;

}
