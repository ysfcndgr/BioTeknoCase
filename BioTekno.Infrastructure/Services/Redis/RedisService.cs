using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BioTekno.Infrastructure.Services.Redis
{
    public class RedisService
    {

    private readonly ConnectionMultiplexer _redis;

    public RedisService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public async Task SetAsync(string key, object value, TimeSpan expiry)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
    }
   }
}

