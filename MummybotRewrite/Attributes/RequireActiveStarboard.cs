using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Services;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Attributes
{
    class RequireActiveStarboard : CheckBaseAttribute
    {
        public override async Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var guildconfig = await provider.GetService<GuildService>().GetGuildAsync(context.Guild);

            if (guildconfig.UsesStarboard)
                return CheckResult.Successful;
            else
                return new CheckResult($"This command can only be used when the bday service is activate.");
        }
    }
}
