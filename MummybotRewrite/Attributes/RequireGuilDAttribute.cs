using Qmmands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    class RequireGuilDAttribute : CheckBaseAttribute
    {
        private ulong _GuildID { get; set; }

        public RequireGuilDAttribute(ulong ID)
    => _GuildID = ID;

        public override Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var Guild =  context.Client.GetGuild(_GuildID);
            if (Guild?.Id == context.Guild.Id)
                return Task.FromResult(CheckResult.Successful);
            else
                return Task.FromResult(new CheckResult($"This command can only be used in {Guild?.Name}"));
        }




        //public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        //{
        //    var Guild = await context.Client.GetGuildAsync(_GuildID);
        //    if (Guild.Id == context.Guild.Id)
        //        return PreconditionResult.FromSuccess();
        //    else
        //        return PreconditionResult.FromError($"This command can only be used in {Guild.Name}");
        //}
    }
}