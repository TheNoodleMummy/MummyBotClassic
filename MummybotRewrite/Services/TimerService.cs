﻿using Mummybot.Attributes;
using Mummybot.interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Mummybot.Services
{
    [Service("Timer Service",typeof(TimerService))]
    public class TimerService
    {
        private readonly Timer _timer;

        [Inject]
        private readonly LogService logs;
        private static TimeSpan MaxTime =>
            TimeSpan.FromMilliseconds(Math.Pow(2, 32) - 2);

        public ConcurrentQueue<IRemoveable> Queue = new ConcurrentQueue<IRemoveable>();

        public TimerService()
        {
            _timer = new Timer(async _ =>
            {
                try
                {
                    if (!Queue.TryDequeue(out var removeable)) return;
                    await HandleRemoveableAsync(removeable);
                    await SetTimerAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }, null,
                TimeSpan.FromMilliseconds(-1),
                TimeSpan.FromMilliseconds(-1));
        }

        public async Task EnqueueAsync(IRemoveable removeable)
        {
            if (removeable is DelayedRemovable delayed)
                removeable = delayed.Removeable;

            //Timer can't have has a value greater than TimeSpan.FromMilliseconds(Math.Pow(2,32) - 2)
            if (removeable.When - DateTime.UtcNow > MaxTime)
                removeable = new DelayedRemovable(removeable);

            Queue.Enqueue(removeable);
            await SetTimerAsync();
        }

        private Task SetTimerAsync()
        {
            try
            {
                if (Queue.IsEmpty) return Task.CompletedTask;

                //Added some overhead to try avoid the very small chance of a race condition
                var toRemove = Queue.Where(x =>
                    x.When.ToUniversalTime() - DateTime.UtcNow <
                    TimeSpan.FromSeconds(10)).ToArray();

                Queue = new ConcurrentQueue<IRemoveable>(Queue
                    .Where(x => x.When.ToUniversalTime() - DateTime.UtcNow > TimeSpan.FromSeconds(10))
                    .OrderBy(x => x.When));

                if (toRemove.Length > 0)
                {
                    //Stops potential race condition
                    Task.Run(async () =>
                    {
                        foreach (var item in toRemove)
                            await item.RemoveAsync();
                    });
                }

                if (Queue.TryPeek(out var removeable))
                    _timer.Change(removeable.When.ToUniversalTime() - DateTime.UtcNow, TimeSpan.FromMilliseconds(-1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }

        private static Task HandleRemoveableAsync(IRemoveable removeable)
            => removeable.RemoveAsync();

        private Task RemoveAsync(IRemoveable obj)
        {
            var newQueue = new ConcurrentQueue<IRemoveable>();
            foreach (var item in Queue)
            {
                if (item.Identifier == obj.Identifier) continue;
                newQueue.Enqueue(item);
            }

            Queue = newQueue;
            return Task.CompletedTask;
        }

        public void RemoveRange(IEnumerable<IRemoveable> objs)
        {
            var newCol = Queue.Except(objs);
            Queue = new ConcurrentQueue<IRemoveable>(newCol);
            SetTimerAsync();
        }

        public async Task UpdateAsync(IRemoveable removeable)
        {
            await RemoveAsync(removeable);
            await EnqueueAsync(removeable);
            await SetTimerAsync();
        }

        private class DelayedRemovable : IRemoveable
        {
            public IRemoveable Removeable { get; }

            public int Identifier => throw new NotImplementedException();
            public DateTime When { get; }

            public DelayedRemovable(IRemoveable removeable)
            {
                Removeable = removeable;
                When = DateTime.UtcNow.Add(MaxTime);
            }

            public Task RemoveAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}