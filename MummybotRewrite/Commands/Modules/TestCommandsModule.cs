using Mummybot.Attributes.Checks;
using Mummybot.Enums;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireOwner]
	public class TestCommandModule : MummyModule
    {
        [Command("test")]
        [Cooldown(1,1,CooldownMeasure.Hours,CooldownBucketType.User)]
		public async Task Test()
        {
            await ReplyAsync(Context.Message.Content);
        }
    }
}