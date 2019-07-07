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
        private readonly TimeSpan _maxTime = TimeSpan.FromMilliseconds(int.MaxValue);

        public readonly ConcurrentQueue<IScheduledTask> Queue;
        private CancellationTokenSource _cts;

        private readonly object _queueLock;

        private IScheduledTask _currentTask;

        public TaskQueue()
        {
            Queue = new ConcurrentQueue<IScheduledTask>();
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
            while (!_disposed)
            {
                try
                {
                    bool wait;

                    lock (_queueLock)
                        wait = !Queue.TryDequeue(out _currentTask);

                    if (wait)
                        await Task.Delay(-1, _cts.Token);

                    var time = _currentTask.ExecutionTime - DateTimeOffset.UtcNow;

                    while (time > _maxTime)
                    {
                        await Task.Delay(_maxTime, _cts.Token);
                        time = _currentTask.ExecutionTime - DateTimeOffset.UtcNow;
                    }

                    if (_currentTask.IsCancelled)
                        continue;

                    await _currentTask.ExecuteAsync();
                    _currentTask.Completed();
                }
                catch (TaskCanceledException)
                {
                    lock (_queueLock)
                    {
                        if (_currentTask?.IsCancelled == false)
                            Queue.Enqueue(_currentTask);

                        if (!Queue.IsEmpty)
                        {
                            var copy = Queue.ToArray().Where(x => !x.IsCancelled).OrderBy(x => x.ExecutionTime);

                            //Didn't do ClearQueue() since nested lock
                            while (Queue.TryDequeue(out _))
                            {
                            }

                            foreach (var item in copy)
                                Queue.Enqueue(item);
                        }

                        _cts.Dispose();
                        _cts = new CancellationTokenSource();
                    }
                }
                catch (Exception e)
                {
                    _currentTask?.SetException(e);

                    OnError?.Invoke(e);
                }
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
