using System;
using System.Threading.Tasks;
using Qmmands;
using Discord.WebSocket;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    public interface IReactionCallback
    {
        RunMode RunMode { get; }
        ICriterion<SocketReaction> Criterion { get; }
        TimeSpan? Timeout { get; }
        MummyContext Context { get; }

        Task<bool> HandleCallbackAsync(SocketReaction reaction);
    }
}
