namespace OMR.Core.Helpers.Ioc
{
    using System;

    public interface IIocContainer
    {
        T Create<T>();
        object Create(Type type);
        bool Contains(Type type);
        void Register<T>(Func<object> action);
        void Register(Type type, Func<object> action);
    }
}
