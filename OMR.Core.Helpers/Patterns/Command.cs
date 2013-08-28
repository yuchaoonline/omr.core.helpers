namespace OMR.Core.Helpers.Patterns
{
    using System;
    
    public interface ICommand<T>
    {
        void Execute();
    }

    public sealed class Command<T> : ICommand<T>
    {
        private T _target;
        private Action<T> _action;

        public Command(T target, Action<T> action)
        {
            if (action == null)
                throw new ArgumentException("action");

            _target = target;
            _action = action;
        }

        public void Execute()
        {
            _action(_target);
        }

    }


}
