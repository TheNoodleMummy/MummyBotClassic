using Discord;
using Qmmands;
using System.Threading.Tasks;
using System.Linq;
using Mummybot.Extentions;
using System;
using Mummybot.Commands.TypeReaders;
using Mummybot.Attributes.Checks;
using System.Text;
using Discord.WebSocket;
using Mummybot.Database.Entities;

namespace Mummybot.Commands.Modules
{
    [Group("config")]
    [RequirePermissions(Enums.PermissionTarget.User, GuildPermission.Administrator, Group = "or")]
    [RequirePermissions(Enums.PermissionTarget.User, GuildPermission.ManageGuild, Group = "or")]
    public class ConfigModule : MummyModule
    {
        [Group("hangman")]
        public class hangmanCommands : MummyModule
        {
            [Command()]
            public async Task OffensiveCommandsOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesHangman = onoff;
                await Context.Message.AddOkAsync();
            }
        }

        [Group("offensive")]
        public class OffensiveCommands : MummyModule
        {
            [Command("commands")]
            public async Task OffensiveCommandsOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.AllowOffensiveCommands = onoff;
                await Context.Message.AddOkAsync();
            }
        }

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
            public async Task SetBirthdaysOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesBirthdays = onoff;
                await Context.Message.AddOkAsync();
            }

            [Command("setchannel")]
            public async Task SetId(ITextChannel channel)
            {
                PermValue? permissions = channel.GetPermissionOverwrite(Context.Guild.CurrentUser.Roles.OrderByDescending(r => r.Position).FirstOrDefault())?.ViewChannel;
                if (permissions is null || permissions == PermValue.Deny)
                {
                    await ReplyAsync("this channel cannot be used as Birthday channel (i cannot see this channel)");
                }
                else
                {
                    GuildConfig.BdayChannelId = channel.Id;
                    await Context.Message.AddOkAsync();
                }
            }
        }

        [Group("voice")]
        public class musicConfigModule : MummyModule
        {
            [Command("music")]
            public async Task SetmusicOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesMusic = onoff;
                await Context.Message.AddOkAsync();
            }

            [Command("trolls")]
            public async Task SettrollsOnOff([OverrideTypeParser(typeof(BoolTypeReader))]bool onoff)
            {
                GuildConfig.UsesTrolls = onoff;
                await Context.Message.AddOkAsync();
            }

            [Group("whitelist")]
            public class WhitelistConfigModule : MummyModule
            {
                [Command("add")]
                public async Task SetmusicOnOff(SocketGuildUser user)
                {
                    GuildConfig.PlayListWhiteLists.Add(new PlayListWhiteList() { UserId = user.Id, WhiteListedBy = Context.UserId });
                    await Context.Message.AddOkAsync();
                }

                [Command("remove")]
                public async Task SettrollsOnOff(SocketGuildUser user)
                {
                    var whitelistentry = GuildConfig.PlayListWhiteLists.FirstOrDefault(x => x.UserId==user.Id);
                    GuildConfig.PlayListWhiteLists.Remove(whitelistentry);
                    await Context.Message.AddOkAsync();
                }
            }
        }


        [Command]
        public async Task GetCOnfig()
        {
            var emb = new EmbedBuilder();
            bool idk=false;
            foreach (var item in GuildConfig.GetType().GetProperties().Where(p => p.PropertyType == typeof(bool)
                                         && (bool)p.GetValue(idk, null)))
            {
                emb.AddField(item.Name, item.GetValue(true));
            }
            await ReplyAsync(embed: emb);
        }
    }
}

