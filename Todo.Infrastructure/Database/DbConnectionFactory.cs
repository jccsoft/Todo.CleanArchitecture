using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Setup;

namespace Todo.Infrastructure.Database;

internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly MySqlConnection? _mySqlConnection;
    private readonly NpgsqlConnection? _postGreSqlConnection;
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;

        if (Config.IsDbMySQL)
        {
            _mySqlConnection = new(connectionString);
        }
        else
        {
            _postGreSqlConnection = new(connectionString);
        }
    }
    public IDbConnection GetOpenConnection()
    {
        if (Config.IsDbMySQL)
        {
            return _mySqlConnection!;
        }
        else
        {
            return _postGreSqlConnection!;
        }

    }

    public IDbConnection CreateNewConnection()
    {
        if (Config.IsDbMySQL)
        {
            return new MySqlConnection(_connectionString);
        }
        else
        {
            return new NpgsqlConnection(_connectionString);
        }

    }
}
