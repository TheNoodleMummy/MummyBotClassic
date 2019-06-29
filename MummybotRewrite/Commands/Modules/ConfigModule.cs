using Discord;
using Qmmands;
using System.Threading.Tasks;
using System.Linq;
using Mummybot.Extentions;
using System;
using Mummybot.Commands.TypeReaders;

namespace Mummybot.Commands.Modules
{
    [Group("config")]
    public class ConfigModule : MummyBase
    {
        [Group("starboard")]
        public class StarboardModule: MummyBase
        {
            [Command("setchannel")]
            public async Task SetId(ITextChannel channel)
            {
                PermValue? permissions = channel.GetPermissionOverwrite(Context.Guild.CurrentUser.Roles.OrderByDescending(r => r.Position).FirstOrDefault())?.ViewChannel;
                if (permissions is null || permissions == PermValue.Deny)
                {
                    await ReplyAsync("this channel cannot be used as starboard (i cannot see this channel)");
                }
                else
                {
                    GuildConfig.StarboardChannelId = channel.Id;
                    await Context.Message.AddOkAsync();
                }
            }

            [Command("setemote")]
            public async Task SetEmote(string emote)
            {
                Emoji emoji = null;
                try
                {
                    emoji = new Emoji(emote);
                }
                catch { }
                if (Emote.TryParse(emote, out Emote _) || emoji != null)
                {
                    GuildConfig.StarboardEmote = emote;
                    await Context.Message.AddOkAsync();
                }
                else
                {
                    await Context.Message.AddNotOkAsync();
                }
            }

            [Command]
            public async Task SetStarBoardOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesStarBoard = onoff;
                await Context.Message.AddOkAsync();
            }
        }

        [Group("Quotes")]
        public class AutoQuotes : MummyBase
        {
            [Command]
            public async Task SetQuotesOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.AutoQuotes = onoff;
                await Context.Message.AddOkAsync();
            }
        }
    }
}

