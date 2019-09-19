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
    [Name("Require Music Service")]
    public class RequireMusicAttribute : MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            using var guildstore = context.ServiceProvider.GetRequiredService<GuildStore>();
            var guild = await guildstore.GetOrCreateGuildAsync(context.GuildId);
            if (guild.UsesMusic)
                return CheckResult.Successful;
            else
                return CheckResult.Unsuccessful("Music Service is Currently turn off for this guild");
        }
    }
}
