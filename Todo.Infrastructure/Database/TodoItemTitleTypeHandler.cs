using Dapper;
using System.Data;

namespace Todo.Infrastructure.Database;

internal sealed class TodoItemTitleTypeHandler : SqlMapper.TypeHandler<TodoItemTitle>
{
    public override TodoItemTitle Parse(object value) => new(value.ToString());

    public override void SetValue(IDbDataParameter parameter, TodoItemTitle? value)
    {
        parameter.Value = value is null ? "" : value.Value;
    }
}