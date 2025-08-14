using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductAPI.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConfiguration configuration, ILogger<RedisCacheService> logger)
        {
            _logger = logger;
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]);
            _cache = redis.GetDatabase();
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, json, expiry);
            _logger.LogInformation("Cache set for key: {Key}", key);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
            _logger.LogInformation("Cache remove for key: {Key}", key);
        }

        public bool KeyExists(string key)
        {
            return _cache.KeyExists(key);
        }
    }
}
