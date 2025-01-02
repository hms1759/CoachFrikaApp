using System.Collections.Concurrent;
using System.Threading.Channels;

namespace CoachFrika.APIs.Domin.BackgroundServices
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);
        ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _channel = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _trackingQueue = new();

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _trackingQueue.Enqueue(workItem); // Track the task in the queue
            await _channel.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _channel.Reader.ReadAsync(cancellationToken);
            _trackingQueue.TryDequeue(out _); // Remove the task from the tracking queue
            return workItem;
        }

        // Get a snapshot of the currently queued tasks
        public int GetQueuedTaskCount() => _trackingQueue.Count;
    }

}
