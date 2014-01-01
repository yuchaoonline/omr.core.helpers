using System;
using System.Runtime.Caching;

namespace OMR.Core.Helpers.Cache
{
    public class InMemoryCache : ICache
    {
        private ObjectCache _cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (!Contains(key))
                return defaultValue;

            return (T)_cache[key];
        }


        public void Set(string key, object value, int timeInSeconds)
        {
            if (value == null)
                return;

            var cip = new CacheItemPolicy();
            cip.AbsoluteExpiration = DateTime.Now.AddSeconds(timeInSeconds);

            _cache.Set(key, value, cip);
        }

        public T GetOrSet<T>(string key, Func<T> function, int timeInSeconds)
        {
            if (!Contains(key))
            {
                Set(key, function(), timeInSeconds);
            }

            return Get<T>(key, default(T));
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Clear(string key)
        {
            foreach (var item in _cache)
            {
                Remove(item.Key);
            }
        }
    }
}
