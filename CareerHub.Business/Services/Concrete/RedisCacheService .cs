using CareerHub.Business.Services.Abstract;
using StackExchange.Redis;

namespace CareerHub.Business.Services.Concrete
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _cache = redisConnection.GetDatabase();
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expire)
        {
          return  await _cache.StringSetAsync(key, value, expire);
           
        }
    }
}
