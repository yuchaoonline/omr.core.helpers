namespace OMR.Core.Helpers.Patterns
{
    using System;
    using System.Collections.Generic;

    public class FlyweightFactory
    {
        private Dictionary<Type, Func<object>> _dictionary;

        public FlyweightFactory()
        {
            _dictionary = new Dictionary<Type, Func<object>>();
        }

        public void Register<T>(Func<object> function) where T : Type
        {
            if (function == null)
                throw new ArgumentException("function");

            _dictionary.Add(typeof(T), function);
        }

        public object Create<T>() where T : IComparable
        {
            if (!_dictionary.ContainsKey(typeof(T)))
                throw new KeyNotFoundException();

            return _dictionary[typeof(T)];
        }

    }
}