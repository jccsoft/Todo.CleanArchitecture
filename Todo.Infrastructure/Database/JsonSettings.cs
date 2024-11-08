using Newtonsoft.Json;

namespace Todo.Infrastructure.Database;

internal static class JsonSettings
{
    public static JsonSerializerSettings SerializerSettings = new() { TypeNameHandling = TypeNameHandling.All };
}
