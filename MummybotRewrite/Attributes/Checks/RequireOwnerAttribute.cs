using Mummybot.Commands;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Attributes.Checks
{
    class RequireOwnerAttribute : CheckAttribute
    {
        public override async ValueTask<CheckResult> CheckAsync(CommandContext context, IServiceProvider provider)
        {
            var ctx = context as MummyContext;
            if ((await ctx.Client.GetApplicationInfoAsync()).Owner.Id == ctx.User.Id)
                return CheckResult.Successful;
            else
                return CheckResult.Unsuccessful("Only my owner can run this command");
        }
    }
}
