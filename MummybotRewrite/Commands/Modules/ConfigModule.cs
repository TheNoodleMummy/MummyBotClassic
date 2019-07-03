using Discord;
using Qmmands;
using System.Threading.Tasks;
using System.Linq;
using Mummybot.Extentions;
using System;
using Mummybot.Commands.TypeReaders;
using Mummybot.Attributes.Checks;

namespace Mummybot.Commands.Modules
{
    [Group("config")]
    [RequirePermissions(Enums.PermissionTarget.User,GuildPermission.Administrator,Group = "or")]
    [RequirePermissions(Enums.PermissionTarget.User,GuildPermission.ManageGuild,Group = "or")]
    public class ConfigModule : MummyModule
    {
        [Group("starboard")]
        public class StarboardModule : MummyModule
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
        public class AutoQuotes : MummyModule
        {
            [Command]
            public async Task SetQuotesOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.AutoQuotes = onoff;
                await Context.Message.AddOkAsync();
            }
        }

        [Group("reminders")]
        public class ReminderConfigModule : MummyModule
        {
            [Command]
            public async Task SetReminderOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesReminders = onoff;
                await Context.Message.AddOkAsync();
            }
        }

        [Group("tags")]
        public class TagsConfigModule : MummyModule
        {
            [Command]
            public async Task SettagOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesTags = onoff;
                await Context.Message.AddOkAsync();
            }
        }

        [Group("birthdays")]
        public class BirthdaysConfigModule : MummyModule
        {
            [Command]
            public async Task SetReminderOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesReminders = onoff;
                await Context.Message.AddOkAsync();
            }
        }
    }
}

