using System;
using System.Threading.Tasks;

namespace Casino.Common
{
    public interface IScheduledTask
    {
        ulong ID { get; }
        bool IsCancelled { get; }
        Exception Exception { get; }
        DateTimeOffset ExecutionTime { get; }

        void Cancel();
        void Completed();
        void SetException(Exception ex);
        Task ExecuteAsync();
    }
}
