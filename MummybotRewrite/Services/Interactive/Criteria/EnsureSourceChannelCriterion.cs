using System.Threading.Tasks;
using Discord.Commands;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    public class EnsureSourceChannelCriterion : ICriterion<IMessage>
    {
        public Task<bool> JudgeAsync(MummyContext sourceContext, IMessage parameter)
        {
            var ok = sourceContext.Channel.Id == parameter.Channel.Id;
            return Task.FromResult(ok);
        }
    }
}
