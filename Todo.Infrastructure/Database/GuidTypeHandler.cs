using Dapper;
using System.Data;

namespace Todo.Infrastructure.Database;

internal sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value) => Guid.Parse((string)value);

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}
