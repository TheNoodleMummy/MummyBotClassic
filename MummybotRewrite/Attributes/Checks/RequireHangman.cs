﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Qmmands;

namespace Mummybot.Attributes.Checks
{
    class RequireHangmanAttribute : MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            using var guildstore = context.ServiceProvider.GetRequiredService<GuildStore>();
            var guild = await guildstore.GetOrCreateGuildAsync(context.GuildId);
            if (guild.UsesHangman)
                return CheckResult.Successful;
            else
                return CheckResult.Failed("Hangman Service is Currently turn off for this guild");


        }
    }
}
