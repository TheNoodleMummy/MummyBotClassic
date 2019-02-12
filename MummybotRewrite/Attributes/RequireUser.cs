using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    public class RequireUserAttribute : CheckBaseAttribute
    {
        private ulong Userid { get; set; }
        public RequireUserAttribute(ulong id)
        {
            Userid = id;
        }
        public override Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var user = context.Client.GetUser(Userid);
            if (Userid == context.User.Id)
                return Task.FromResult(CheckResult.Successful);
            else
                return Task.FromResult(new CheckResult($"you are not allowed to use this command only {user.Username} can."));
        }

        //public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        //{
        //    var user = await context.Client.GetUserAsync(Userid);
        //    if (Userid == context.User.Id)
        //        return PreconditionResult.FromSuccess();
        //    else
        //        return PreconditionResult.FromError($"you are not allowed to use this command only {user.Username} can.");

        //}

    }
}
