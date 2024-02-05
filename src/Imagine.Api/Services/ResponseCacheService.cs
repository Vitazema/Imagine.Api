using System.Text.Json;
using StackExchange.Redis;

namespace Imagine.Api.Services;

public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
{
    private readonly IDatabase _redis = redis.GetDatabase();

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response == null) return;

        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedResponse = JsonSerializer.Serialize(response, options);

        await _redis.StringSetAsync(cacheKey, serializedResponse, timeToLive);
    }

    public async Task<string> GetCachedResponseAsync(string cacheKey)
    {
        var cachedResponse = await _redis.StringGetAsync(cacheKey);
        if (cachedResponse.IsNullOrEmpty) return null;

        return cachedResponse;
    }
}
