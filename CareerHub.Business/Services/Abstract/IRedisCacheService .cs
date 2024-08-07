using System.Composition;

namespace CareerHub.Business.Services.Abstract
{
    public interface IRedisCacheService
    {   
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value, TimeSpan? expire);
    }
}
