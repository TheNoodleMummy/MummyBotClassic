using Qmmands;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Temp Commands"), Description("holds some commands that will go away again after time or commands that are in test phase")]
    public class TempCommands : MummyBase
    {
        [Command("ulong")]
        public async Task getulong(long nr = 0)
        {

            ulong result;
            unchecked
            {
                result = (ulong)(nr + long.MaxValue);
            }
            await ReplyAsync(result.ToString());
        }
    }
}