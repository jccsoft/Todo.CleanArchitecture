namespace Todo.Infrastructure.Setup;

public static class Config
{
    private static readonly CacheTypes _cacheType = CacheTypes.InMemory;
    private static readonly DatabaseTypes _databaseType = DatabaseTypes.MySQL;
    private static readonly ORMTypes _ormType = ORMTypes.Dapper;


    public static bool IsDbMySQL => _databaseType == DatabaseTypes.MySQL;
    public static bool IsDbPostgreSQL => _databaseType == DatabaseTypes.PostgreSQL;
    public static string DbConnectionStringName => IsDbMySQL ? "MySQL" : "PostgreSQL";

    public static bool IsOrmDapper => _ormType == ORMTypes.Dapper;
    public static bool IsOrmEFCore => _ormType == ORMTypes.EFCore;

    public static bool IsCacheInMemory => _cacheType == CacheTypes.InMemory;
    public static bool IsCacheRedis => _cacheType == CacheTypes.Redis;

}
