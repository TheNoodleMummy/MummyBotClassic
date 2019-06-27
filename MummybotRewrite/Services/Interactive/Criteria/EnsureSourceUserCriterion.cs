using System.Threading.Tasks;
using Discord.Commands;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    public class EnsureSourceUserCriterion : ICriterion<IMessage>
    {
        public Task<bool> JudgeAsync(MummyContext sourceContext, IMessage parameter)
        {
            var ok = sourceContext.User.Id == parameter.Author.Id;
            return Task.FromResult(ok);
        }
    }
}
