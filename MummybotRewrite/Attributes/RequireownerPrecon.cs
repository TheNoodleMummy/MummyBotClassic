using Discord;
using Mummybot.Commands;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Attributes
{
    public class RequireOwnerAttribute : CheckBaseAttribute
    {
        public override async Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            switch (context.Client.TokenType)
            {
                case TokenType.Bot:
                    var application = await context.Client.GetApplicationInfoAsync();
                    if (context.User.Id != application.Owner.Id)
                        return await Task.FromResult(new CheckResult("Command can only be run by the owner of the bot"));
                    return await Task.FromResult(CheckResult.Successful);

                default:
                    return await Task.FromResult(new CheckResult($"{nameof(RequireOwnerAttribute)} is not supported by this {nameof(TokenType)}."));
            }
        }
    }
}
