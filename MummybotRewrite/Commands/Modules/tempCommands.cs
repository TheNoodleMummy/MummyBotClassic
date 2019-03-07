using Qmmands;
using System.Text;
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


        [Command("emotes")]
        public async Task Emotes()
        {

            var sb = new StringBuilder();
            foreach (var guild in Context.Client.Guilds)
            {
                foreach (var emote in guild.Emotes)
                {
                    sb.Append($"<:{emote.Name}:{emote.Id}>");
                }
            }
            await ReplyAsync(sb.ToString());
        }
    }
}