﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Common
{
    public sealed partial class TaskQueue
    {
        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <typeparam name="T">The type you want your object in the callback to be.</typeparam>
        /// <param name="state">The object that you want to access in your callback.</param>
        /// <param name="executeIn">How long to wait before execution.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask{T}"/></returns>
        public ScheduledTask<T> ScheduleTask<T>(T state, TimeSpan executeIn, Func<T, Task> callback, ulong id = 0)
        {
            ArgChecks(executeIn);
            return ScheduleTask(state, DateTimeOffset.UtcNow.Add(executeIn), callback, id);
        }

        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <typeparam name="T">The type you want your object in the callback to be.</typeparam>
        /// <param name="state">The object that you want to access in your callback.</param>
        /// <param name="whenToExecute">The time at when this task needs to be ran.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask{T}"/></returns>
        public ScheduledTask<T> ScheduleTask<T>(T state, DateTimeOffset whenToExecute, Func<T, Task> callback, ulong id = 0)
        {
            ArgChecks(whenToExecute, callback);

            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                var toAdd = new ScheduledTask<T>(this, state, whenToExecute, callback, id);

                Queue.Enqueue(toAdd);
                _cts.Cancel(true);

                return toAdd;
            }
        }

        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <param name="state">The object that you want to access in your callback.</param>
        /// <param name="executeIn">How long to wait before execution.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask"/></returns>
        public ScheduledTask ScheduleTask(object state, TimeSpan executeIn, Func<object, Task> callback)
        {
            ArgChecks(executeIn);
            return ScheduleTask(state, DateTimeOffset.UtcNow.Add(executeIn), callback);
        }

        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <param name="state">The object that you want to access in your callback.</param>
        /// <param name="whenToExecute">The time at when this task needs to be ran.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask"/></returns>
        public ScheduledTask ScheduleTask(object state, DateTimeOffset whenToExecute, Func<object, Task> callback)
        {
            ArgChecks(whenToExecute, callback);

            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                var toAdd = new ScheduledTask(this, state, whenToExecute, callback);

                Queue.Enqueue(toAdd);
                _cts.Cancel(true);

                return toAdd;
            }
        }

        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <param name="executeIn">How long to wait before execution.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask"/></returns>
        public ScheduledTask ScheduleTask(TimeSpan executeIn, Func<Task> callback)
        {
            ArgChecks(executeIn);
            return ScheduleTask(DateTimeOffset.UtcNow.Add(executeIn), callback);
        }

        /// <summary>
        /// Schedules a new task.
        /// </summary>
        /// <param name="whenToExecute">The time at when this task needs to be ran.</param>
        /// <param name="callback">The task to be executed.</param>
        /// <returns>A <see cref="ScheduledTask"/></returns>
        public ScheduledTask ScheduleTask(DateTimeOffset whenToExecute, Func<Task> callback)
        {
            if (whenToExecute - DateTimeOffset.UtcNow < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(whenToExecute));

            if (callback is null)
                throw new ArgumentNullException(nameof(callback));

            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                var toAdd = new ScheduledTask(this, null, whenToExecute, _ => callback());

                Queue.Enqueue(toAdd);
                _cts.Cancel(true);

                return toAdd;
            }
        }

        private void ArgChecks<T>(DateTimeOffset whenToExecute, Func<T, Task> callback)
        {
            if (whenToExecute - DateTimeOffset.UtcNow < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(whenToExecute));

            if (callback is null)
                throw new ArgumentNullException(nameof(callback));
        }

        private void ArgChecks(TimeSpan executeIn)
        {
            if (executeIn < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(executeIn));
        }

        /// <summary>
        /// Clears and cancels all the currently scheduled tasks from the queue.
        /// </summary>
        public void ClearQueue()
        {
            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                CurrentTask.Cancel();

                while (Queue.TryDequeue(out var task))
                {
                    task.Cancel();
                }

                _cts.Cancel(true);
            }
        }

        /// <summary>
        /// Disposes of the <see cref="TaskQueue" /> and frees up any managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        public void Remove(IScheduledTask scheduledTask)
        {
            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                Queue = new ConcurrentQueue<IScheduledTask>(Queue.Where(task => task.ID != scheduledTask.ID));
                _cts.Cancel(true);
            }
        }

        internal void Reschedule()
        {
            lock (_queueLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(TaskQueue));

                _cts.Cancel(true);
            }
        }
    }
}
