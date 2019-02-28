using Discord;
using Mummybot.Attributes;
using Qmmands;
using System;
using System.IO;
using System.Linq;
using System.Net;
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
            string url = Context.Message.Attachments.First().Url.ToString();
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(url);

            // returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;
            using (HttpWebResponse lxResponse = (HttpWebResponse)await lxRequest.GetResponseAsync())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    using (FileStream lxFS = new FileStream("configs/icon.png", FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }
            await Task.Delay(1500);

            await Context.Guild.ModifyAsync(x => x.Icon = new Image(@"configs/icon.png"));

        }



    }
}
