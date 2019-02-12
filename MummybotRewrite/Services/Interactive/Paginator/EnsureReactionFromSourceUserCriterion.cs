using System.Threading.Tasks;
using Qmmands;
using Discord.WebSocket;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    internal class EnsureReactionFromSourceUserCriterion : ICriterion<SocketReaction>
    {
        public Task<bool> JudgeAsync(ICommandContext ctx, SocketReaction parameter)
        {
            var context = ctx as MummyContext;
            bool ok = parameter.UserId == context.User.Id;
            return Task.FromResult(ok);
        }
    }
}
