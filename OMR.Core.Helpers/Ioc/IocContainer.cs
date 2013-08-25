namespace OMR.Core.Helpers.Ioc
{
    using System; 
    using System.Collections.Generic;

    public class IocContainer : IIocContainer
    {
        private readonly Dictionary<Type, Func<object>> _funcBased = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, Type> _typeBased = new Dictionary<Type, Type>();

        public void Register<T>(Func<object> action)
        {
            if (Contains(typeof(T)))
                throw new InvalidOperationException("Type already exists");

            _funcBased.Add(typeof(T), action);
        }

        public void Register(Type type, Func<object> action)
        {
            if (Contains(type))
                throw new InvalidOperationException("Type already exists");

            _funcBased.Add(type, action);
        }

        public void Register<T1, T2>()
        {
            if (Contains(typeof(T1)))
                throw new InvalidOperationException("Type already exists");

            _typeBased.Add(typeof(T1), typeof(T2));
        }

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        public object Create(Type type)
        {
            if (_funcBased.ContainsKey(type))
            {
                return _funcBased[type]();
            }
            else if (_typeBased.ContainsKey(type))
            {
                var targetType = _typeBased[type];
                var targetTypeInstance = Activator.CreateInstance(targetType);

                return targetTypeInstance;
            }
            else
            {
                throw new InvalidOperationException("Type can not be found in the container");
            }
        }

        public bool Contains(Type type)
        {
            return _funcBased.ContainsKey(type);
        }
    }
}
