namespace OMR.Core.Helpers.Patterns
{
    using System;

    public interface IChainOfResponsbility<T>
    {
        Func<object, bool> Handle { get; }

        IChainOfResponsbility<T> Successor { get; }
    }

    public class ChainOfResponsbility<T, K> where T : IChainOfResponsbility<T>
    {

        public void Handle(T responser, K item)
        {
            if (responser == null || responser.Handle == null)
                throw new InvalidOperationException("There was no response or handler for this operation");

            if (!responser.Handle(item))
            {
                Handle((T)responser.Successor, item);
            }
        }

    }
}
