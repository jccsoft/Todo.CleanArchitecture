using MySql.Data.MySqlClient;
using System.Data;
using Todo.Application.Abstractions.Data;

namespace Todo.Infrastructure.Database;

internal sealed class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    readonly MySqlConnection _mySqlConnection = new(connectionString);

    public IDbConnection GetOpenConnection()
    {
        return _mySqlConnection;
    }

    public IDbConnection CreateNewConnection()
    {
        return new MySqlConnection(connectionString);
    }
}
