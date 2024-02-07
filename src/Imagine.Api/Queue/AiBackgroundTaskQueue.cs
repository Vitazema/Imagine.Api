using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace Imagine.Api.Queue;

public class AiBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Art> _queue;

    public AiBackgroundTaskQueue()
    {
        BoundedChannelOptions options = new(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Art>(options);
    }

    public async ValueTask EnqueueAsync([NotNull] Art art)
    {
        await _queue.Writer.WriteAsync(art);
    }

    public async ValueTask<Art> DequeueAsync(CancellationToken cancellationToken)
    {
        var item = await _queue.Reader.ReadAsync(cancellationToken);
        return item;
    }
}
