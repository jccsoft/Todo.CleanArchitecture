using System.Data;

namespace Todo.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    IDbConnection GetOpenConnection();
    IDbConnection CreateNewConnection();
}
