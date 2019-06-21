using Discord;
using Mummybot.Attributes;
using Qmmands;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{

    [Name("Guild Control Module"), Description("Contains commands to modify the guild"), RequireUserPermission(GuildPermission.ManageGuild)]
    public class GuildControlModule : MummyBase
    {


        [Command("guildicon")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetGuildIcon()
        {
            string url = Context.Message.Attachments.First().Url;
            var stream = await Context.HttpClient.GetStreamAsync(url);
            await Context.Guild.ModifyAsync(x => x.Icon = new Image(stream));
        }



    }
}
