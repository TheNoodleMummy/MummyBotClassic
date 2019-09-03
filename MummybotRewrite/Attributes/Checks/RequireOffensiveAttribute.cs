using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Qmmands;

namespace Mummybot.Attributes.Checks
{
    [Name("Require Offensive commands")]
    class RequireOffensiveAttribute : MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context, IServiceProvider provider)
        {
            using var guildstore = provider.GetRequiredService<GuildStore>();
            var guild = await guildstore.GetOrCreateGuildAsync(context.Guild);

            if (guild.AllowOffensiveCommands)
            {
                return new CheckResult();
            }
            else
            {
                return new CheckResult("Offensive commands are not active in this guild " +
                    "\n(you can ask a GuildModerator to turn them on however im not responsible for anyone that might get offended by them)");
            }
        }
    }
}
