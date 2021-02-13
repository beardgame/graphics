using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace amulware.Graphics.Utilities
{
    /// <summary>
    /// A threadsafe queue to run actions from.
    /// Typical usage is to schedule actions from multiple threads and execute them on one main thread.
    /// However, actions can also be executed by multiple threads.
    /// If only one thread is used to execute, the actions are guaranteed to be executed in the order they were scheduled.
    /// </summary>
    sealed class ManualActionQueue
    {
        private readonly BlockingCollection<Action> actions = new BlockingCollection<Action>();

        #region Methods

        #region Execute

        /// <summary>
        /// Executes one scheduled action.
        /// If no action is scheduled, this will wait until one is scheduled, and then execute that.
        /// This will never return without having executed exactly one scheduled action.
        /// </summary>
        public void ExecuteOne()
        {
            var action = this.actions.Take();
            action();
        }

        /// <summary>
        /// Executes one scheduled action, if any are available.
        /// Returns immediately if no actions are scheduled.
        /// </summary>
        /// <returns>Whether an action was executed.</returns>
        public bool TryExecuteOne()
        {
            Action action;
            if (this.actions.TryTake(out action))
            {
                action();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to execute one scheduled action.
        /// If no action is available immediately, it will wait the given time span for an action to be scheduled before returning.
        /// </summary>
        /// <param name="timeout">The time span.</param>
        /// <returns>Whether an action was executed.</returns>
        public bool TryExecuteOne(TimeSpan timeout)
        {
            Action action;
            if (this.actions.TryTake(out action, timeout))
            {
                action();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes scheduled actions for a given time span.
        /// Returns after the first action, if the given time has elapsed.
        /// May run longer than the given time, depending on the scheduled actions, but will never take less.
        /// </summary>
        /// <param name="time">The time span.</param>
        /// <returns>The number of actions executed.</returns>
        public int ExecuteFor(TimeSpan time)
        {
            var timer = Stopwatch.StartNew();
            var executed = 0;
            while (true)
            {
                var timeLeft = time - timer.Elapsed;
                Action action;
                if (timeLeft < new TimeSpan(0))
                    break;
                if (!this.actions.TryTake(out action, timeLeft))
                    break;
                executed++;
                action();
            }
            return executed;
        }

        #endregion

        #region IActionQueue

        /// <summary>
        /// Queues an action to run. Returns immediately.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public void RunAndForget(Action action)
        {
            this.actions.Add(action);
        }

        /// <summary>
        /// Queues an action to run. Returns only after the action has been executed.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public void RunAndAwait(Action action)
        {
            var reset = new ManualResetEvent(false);

            this.actions.Add(() =>
            {
                action();
                reset.Set();
            });

            reset.WaitOne();
        }

        /// <summary>
        /// Queues a parameterless function to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        public T RunAndReturn<T>(Func<T> action)
        {
            T ret = default(T);
            this.RunAndAwait(() => ret = action());
            return ret;
        }

        /// <summary>
        /// Queues a function with one parameter to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The argument for calling the function.</param>
        public T RunAndReturn<TP0, T>(Func<TP0, T> action, TP0 p0)
        {
            return this.RunAndReturn(() => action(p0));
        }

        /// <summary>
        /// Queues a function with two parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, T>(Func<TP0, TP1, T> action, TP0 p0, TP1 p1)
        {
            return this.RunAndReturn(() => action(p0, p1));
        }

        /// <summary>
        /// Queues a function with three parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, TP2, T>(Func<TP0, TP1, TP2, T> action, TP0 p0, TP1 p1, TP2 p2)
        {
            return this.RunAndReturn(() => action(p0, p1, p2));
        }

        /// <summary>
        /// Queues a function with four parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        /// <param name="p3">The fourth argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, TP2, TP3, T>(Func<TP0, TP1, TP2, TP3, T> action, TP0 p0, TP1 p1, TP2 p2, TP3 p3)
        {
            return this.RunAndReturn(() => action(p0, p1, p2, p3));
        }

        #endregion

        #endregion

    }
}
