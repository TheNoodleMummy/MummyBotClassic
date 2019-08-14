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
    class RequireReminders : MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context, IServiceProvider provider)
        {
            using var guildstore = provider.GetRequiredService<GuildStore>();
            var guildconfig = await guildstore.GetOrCreateGuildAsync(context.Guild);
            if (guildconfig.UsesReminders)
                return CheckResult.Successful;
            else
                return CheckResult.Unsuccessful("this command can only be used when the ReminderSerivce is active");

        }
    }
}
