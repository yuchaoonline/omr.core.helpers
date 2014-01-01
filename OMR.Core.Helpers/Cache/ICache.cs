using System;

namespace OMR.Core.Helpers.Cache
{
    public interface ICache
    {
        T Get<T>(string key, T defaultValue);

        void Set(string key, object value, int timeInSeconds);

        T GetOrSet<T>(string key, Func<T> function, int timeInSeconds);

        bool Contains(string key);

        void Remove(string key);

        void Clear(string key);
    }
}
