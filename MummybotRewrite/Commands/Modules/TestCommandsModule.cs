using Mummybot.Attributes.Checks;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireOwner]
	public class TestCommandModule : MummyModule
    {
        [Command("test")]
		public async Task Test([Remainder]DateTimeOffset dateTime)
        {
            await ReplyAsync(dateTime.ToString());
        }
    }
}