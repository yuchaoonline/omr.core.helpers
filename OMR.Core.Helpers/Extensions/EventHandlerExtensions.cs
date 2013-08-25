namespace OMR.Core.Helpers.Extensions
{
    using System;
    using System.Diagnostics;

    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler handler, object sender)
        {
            if (handler == null)
                return;

            handler(sender, EventArgs.Empty);
        }

        [Obsolete()]
        public static void Raise<T>(this EventHandler<T> handler) where T : EventArgs, new()
        {
            var stackFrame = new StackFrame(1);
            var methodBase = stackFrame.GetMethod();

            Raise<T>(handler, methodBase);
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender) where T : EventArgs, new()
        {
            Raise<T>(handler, sender, null);
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, Action<T> action) where T : EventArgs, new()
        {
            if (handler == null)
                return;

            T args = new T();

            if (action != null)
                action(args);

            handler(sender, args);
        }
    }
}
