﻿using Qmmands;
using Mummybot.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Services;
using Mummybot.Commands;

namespace Mummybot.Attributes
{
    class RequireActiveBlackList : CheckBaseAttribute
    {
        //public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        //{
        //    var guildconfig = services.GetService<DBService>().GetGuild(context.Guild);
        //    if (guildconfig.UsesBlackList)          
        //        return PreconditionResult.FromSuccess();
        //    else
        //        return PreconditionResult.FromError($"This command can only be used when the blacklist is activated.");
        //}
        public override async Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var guildconfig = await provider.GetService<GuildService>().GetGuildAsync(context.Guild);
            if (guildconfig.UsesBlackList)
               return CheckResult.Successful;
            else
               return new CheckResult($"This command can only be used when the blacklist is activated.");
        }
    }
}
