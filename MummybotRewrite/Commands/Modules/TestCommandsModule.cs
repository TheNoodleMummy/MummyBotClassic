using Qmmands;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
	public class TestCommandModule : MummyBase
    {
        [Command("test")]
		public async Task Test()
        {
            await ReplyAsync(string.Join(',', Context.User.ActiveClients));
        }
    }
}