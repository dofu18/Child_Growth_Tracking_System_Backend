using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace InfrastructureLayer.Core.Cache
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string key);
        Task Set<T>(string key, T value);
        Task Set<T>(string key, T value, TimeSpan expiration);
        Task<bool> Update<T>(string key, T value);
        Task Remove(string key);
        Task<bool> Exists(string key);
        Task Clear();
        Task ClearWithPattern(string pattern);
        Task ForceLogout(Guid userId);
    }
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        private readonly string _redisKey = "childgrowth";

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        public async Task<T?> Get<T>(string key)
        {
            var value = await _database.StringGetAsync($"{_redisKey}:{key}");
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        public Task Set<T>(string key, T value)
        {
            return _database.StringSetAsync($"{_redisKey}:{key}", JsonSerializer.Serialize(value), TimeSpan.FromMinutes(30));
        }

        public Task Set<T>(string key, T value, TimeSpan expiration)
        {
            return _database.StringSetAsync($"{_redisKey}:{key}", JsonSerializer.Serialize(value), expiration);
        }

        public async Task<bool> Update<T>(string key, T value)
        {
            TimeSpan? ttl = await _database.KeyTimeToLiveAsync($"{_redisKey}:{key}");
            return await _database.StringSetAsync($"{_redisKey}:{key}", JsonSerializer.Serialize(value), ttl);
        }

        public Task Remove(string key)
        {
            return _database.KeyDeleteAsync($"{_redisKey}:{key}");
        }

        public Task<bool> Exists(string key)
        {
            return _database.KeyExistsAsync($"{_redisKey}:{key}");
        }

        public Task Clear()
        {
            return _database.ExecuteAsync("FLUSHDB");
        }

        public Task ClearWithPattern(string pattern)
        {
            var endpoints = _database.Multiplexer.GetEndPoints();
            var server = _database.Multiplexer.GetServer(endpoints.First());
            var keys = server.Keys(pattern: $"{_redisKey}:{pattern}*").ToArray();
            return _database.KeyDeleteAsync(keys);
        }

        public Task ForceLogout(Guid userId)
        {
            var redisKey = $"ss:{userId}";

            return ClearWithPattern(redisKey);
        }
    }
}
