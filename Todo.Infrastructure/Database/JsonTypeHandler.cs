using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace Todo.Infrastructure.Database;
internal sealed class JsonTypeHandler : SqlMapper.TypeHandler<string>
{
    public override string Parse(object value) => JsonConvert.DeserializeObject<string>((string)value)!;

    public override void SetValue(IDbDataParameter parameter, string? value)
    {
        parameter.Value = value is null ? "" : JsonConvert.SerializeObject(value);
    }
}