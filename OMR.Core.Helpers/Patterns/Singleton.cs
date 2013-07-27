using System;

namespace OMR.Core.Helpers.Patterns
{
    public class Singleton<T>
    {
        private static object _lockObject = new object();
        private static Func<T> _initiliazer;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_initiliazer == null)
                    return default(T);

                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        _instance = _initiliazer();
                    }
                }

                return _instance;
            }
        }

        private Singleton() { }

        public void Initialize(Func<T> initializer)
        {
            _initiliazer = initializer;
        }

        public void Initialize(T instance)
        {
            _instance = instance;
            _initiliazer = new Func<T>(() => instance);
        }


    }
}