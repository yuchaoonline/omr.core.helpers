namespace OMR.Core.Helpers.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key, T defaultValue);

        void Set(string key, object value, int timeInSeconds);

        bool Contains(string key);

        void Remove(string key);

        void Clear(string key);
    }
}
