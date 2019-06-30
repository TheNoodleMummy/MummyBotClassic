using Mummybot.Services;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Casino.Common
{
    /// <summary>
    /// A simple task scheduler.
    /// </summary>
    public sealed partial class TaskQueue : BaseService, IDisposable
    {
        public readonly ConcurrentQueue<IScheduledTask> _taskQueue;
        private CancellationTokenSource _cts;

        private readonly object _queueLock;

        private IScheduledTask _currentTask;

        public TaskQueue()
        {
            _taskQueue = new ConcurrentQueue<IScheduledTask>();
            _cts = new CancellationTokenSource();

            _queueLock = new object();
            _ = HandleCallbacksAsync();
        }

        /// <summary>
        /// Event that fires whenever there is an exception from a scheduled task.
        /// </summary>
        public event Func<Exception, Task> OnError;

        private async Task HandleCallbacksAsync()
        {
            if (_disposed)
                return;

            try
            {
                bool wait;

                lock (_queueLock)
                    wait = !_taskQueue.TryDequeue(out _currentTask);

                if (wait)
                    await Task.Delay(-1, _cts.Token);

                var time = _currentTask.ExecutionTime - DateTimeOffset.UtcNow;

                if (time > TimeSpan.Zero)
                {
                    await Task.Delay(time, _cts.Token);
                }

                if (_currentTask.IsCancelled)
                    return;

                await _currentTask.ExecuteAsync();
                _currentTask.Completed();
            }
            catch (TaskCanceledException)
            {
                lock (_queueLock)
                {
                    if (_currentTask?.IsCancelled == false)
                        _taskQueue.Enqueue(_currentTask);

                    if (!_taskQueue.IsEmpty)
                    {
                        var copy = _taskQueue.ToArray().Where(x => !x.IsCancelled).OrderBy(x => x.ExecutionTime);

                        //Didn't do ClearQueue() since nested lock
                        while (_taskQueue.TryDequeue(out _))
                        {
                        }

                        foreach (var item in copy)
                            _taskQueue.Enqueue(item);
                    }

                    _cts.Dispose();
                    _cts = new CancellationTokenSource();
                }
            }
            catch (Exception e)
            {
                _currentTask?.SetException(e);

                if (OnError != null)
                    await OnError(e);
            }
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            lock (_queueLock)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _cts.Cancel(true);
                        _cts.Dispose();
                    }

                    _disposed = true;
                }
            }
        }
    }
}
