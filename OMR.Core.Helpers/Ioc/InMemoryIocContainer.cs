using OMR.Core.Helpers.Ioc;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace OMR.Core.Helpers.Ioc
{
    public class InMemoryIocContainer : IIocContainer
    {
        private Hashtable _hashes;

        public InMemoryIocContainer()
        {
            _hashes = new Hashtable();
        }

        public T Get<T>(string name)
        {
            if (!Contains(name))
                throw new InvalidOperationException("Not found");

            var targetType = ((T)_hashes[name]);

            return (T)Activator.CreateInstance(targetType.GetType());
        }

        public T Get<T>()
        {
            if (!Contains<T>())
                throw new InvalidOperationException("Not found");

            var ro = _hashes[typeof(T).FullName];

            TypeConverter tc = new TypeConverter();
            var tx = ro.GetType();
            var instance = Activator.CreateInstance(ro.GetType(), true);
            return (T)instance;
        }

        public bool Contains(string name)
        {
            return _hashes.ContainsKey(name);
        }

        public bool Contains<T>()
        {
            return _hashes.ContainsKey(typeof(T).FullName);
        }

        public void Register(string name, object obj)
        {
            _hashes.Add(name, obj);
        }

        public void Register<T1, T2>()
        {
            _hashes.Add(typeof(T1).FullName, typeof(T2));
        }
    }
}