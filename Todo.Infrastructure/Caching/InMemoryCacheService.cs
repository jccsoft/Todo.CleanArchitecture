using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Buffers;
using System.Text.Json;
using Todo.Application.Abstractions.Caching;

namespace Todo.Infrastructure.Caching;

internal sealed class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cache.TryGetValue(key, out byte[]? bytes);

        return Task.FromResult(bytes is null ? default : Deserialize<T>(bytes));
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);
        cache.Set(key, bytes, expiration ?? CacheOptions.DefaultAbsoluteExpirationRelativeToNow);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cache.Remove(key);

        return Task.CompletedTask;
    }

    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }
}
