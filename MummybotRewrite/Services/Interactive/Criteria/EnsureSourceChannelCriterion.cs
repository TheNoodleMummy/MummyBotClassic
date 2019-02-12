using System.Threading.Tasks;
using Mummybot.Commands;
using Qmmands;

namespace Discord.Addons.Interactive
{
    public class EnsureSourceChannelCriterion : ICriterion<IMessage>
    {
        public Task<bool> JudgeAsync(ICommandContext ctx, IMessage parameter)
        {
            var context = ctx as MummyContext;
            var ok = context.Channel.Id == parameter.Channel.Id;
            return Task.FromResult(ok);
        }
    }
}
