using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Attributes.Checks
{
    class Require18PlusAttribute :MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            using var guildstore = context.ServiceProvider.GetRequiredService<GuildStore>();
            var guild = await guildstore.GetOrCreateGuildAsync(context.Guild);

            if (guild.Allow18PlusCommands)
            {
                return new CheckResult();
            }
            else
            {
                return new CheckResult("18+ commands are not active in this guild " +
                    "\n(you can ask a GuildModerator to turn them on)");
            }
        }
    }
}
