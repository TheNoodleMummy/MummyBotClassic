using System.Threading.Tasks;
using Mummybot.Commands;
using Qmmands;

namespace Discord.Addons.Interactive
{
    public class EnsureSourceUserCriterion : ICriterion<IMessage>
    {
        public Task<bool> JudgeAsync(ICommandContext ctx, IMessage parameter)
        {
            var context = ctx as MummyContext;
            var ok = context.User.Id == parameter.Author.Id;
            return Task.FromResult(ok);
        }
    }
}
