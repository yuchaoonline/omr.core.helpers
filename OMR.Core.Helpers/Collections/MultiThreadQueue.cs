namespace OMR.Core.Helpers.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    /// <summary>
    /// This class implements a collection of multi-threaded queue.
    /// 2012 - OMR
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiThreadQueue<T> : ConcurrentQueue<T>
    {
        private int _maxThreadCount;
        private int _runningThreadCount;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="MaxThreadCount">Maximum count of running thread</param>
        public MultiThreadQueue(int MaxThreadCount)
        {
            _maxThreadCount = MaxThreadCount;
        }

        /// <summary>
        /// Runs multi threaded queue.
        /// </summary>
        /// <param name="action"></param>
        public void RunAll(Action<T> action)
        {
            while (true)
            {
                int queueCount = this.Count;

                // Calculates number of new threads count
                var newThreadCount = (
                                        queueCount > (_maxThreadCount - _runningThreadCount)
                                            ? (_maxThreadCount - _runningThreadCount)
                                            : queueCount
                                    );

                // Creates new threads
                for (int i = 0; i < newThreadCount; i++)
                {
                    T currentWork;

                    if (!this.TryDequeue(out currentWork))
                        continue;

                    if (currentWork == null)
                        continue;

                    Interlocked.Increment(ref _runningThreadCount);

                    var thread = new Thread(
                                        new ParameterizedThreadStart((x) => Run(action, currentWork))
                    );

                    thread.Start();
                }

                if (queueCount == 0 && _runningThreadCount == 0)
                    break;
            }
        }

        private void Run(Action<T> action, T item)
        {
            action(item);

            Interlocked.Decrement(ref _runningThreadCount);
        }

    }
}
