using Qmmands;
using System;
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
    }
}
