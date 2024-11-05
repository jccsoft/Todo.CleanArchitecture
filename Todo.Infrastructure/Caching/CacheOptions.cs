using Microsoft.Extensions.Caching.Distributed;

namespace Todo.Infrastructure.Caching;

public static class CacheOptions
{
    public static TimeSpan DefaultAbsoluteExpirationRelativeToNow { get; } = TimeSpan.FromSeconds(10);

    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = DefaultAbsoluteExpirationRelativeToNow
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration is not null ?
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration } :
            DefaultExpiration;
}
