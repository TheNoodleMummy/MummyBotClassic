using Discord;
using Mummybot.Attributes;
using Mummybot.Enums;
using Mummybot.Services;
using Qmmands;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireOwner]
    public class OwnerModule : MummyBase
    {
        public EvalService EvalService { get; set; }
        public TimerService TimerService { get; set; }


        [Command("timers")]
        public async Task Getimers()
        {
            var sb = new StringBuilder();
            foreach (var item in TimerService.Queue)
            {
                sb.AppendLine($"{item} in {item.When - DateTime.UtcNow}");
            }
            await ReplyAsync(sb.ToString());
        }

        [Group("blacklistguild")]
        public class Blacklisting : MummyBase
        {
            [Command("add"), Description("blacklist a guild from using any bot command"),]
            public async Task Add([Description("guildToBlacklist")]IGuild guild, [Remainder]string reason = null)
            {
                var guildconfig = Database.GetGuildUncached(guild.Id);
                if (guildconfig.IsBlackListed)
                    await Messages.SendMessageAsync(Context, "This guild is already blacklisted");
                else
                {
                    guildconfig.IsBlackListed = true;
                    await GuildService.UpdateGuildAsync(guildconfig);
                    Logs.LogInformation($"{guild} blacklisted in {Context.Guild}/{Context.Guild.Id} ", LogSource.BlackList);
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
            }

            [Command("remove"), Description("remove a user from the blacklist"),]
            public async Task Remove(IGuild guild)
            {
                var guildconfig = Database.GetGuildUncached(guild.Id);

                if (guildconfig.IsBlackListed)
                {
                    guildconfig.IsBlackListed = false;
                    await GuildService.UpdateGuildAsync(guildconfig);
                    Logs.LogInformation($"{guild} removed from blacklist in {Context.Guild}/{Context.Guild.Id} ", LogSource.BlackList);
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
                else
                    await ReplyAsync("this guild is not blacklisted");
            }

            [Command, Description("list all guildIds that have been blacklist")]
            public async Task Listblacklists()
            {
                StringBuilder sb = new StringBuilder();
                var blacklistedguilds = Database.GetAllGuilds().Where(x => x.IsBlackListed);
                foreach (var bguild in blacklistedguilds)
                {
                    var guild = Context.Client.GetGuild(bguild.GuildID);
                    sb.AppendLine($"[{guild.Id}]/{guild?.Name}");
                }
                await ReplyAsync(sb.ToString());
            }
        }

        [Command("notify")]
        public async Task NotifyGuild(ulong guildid, ulong channelid, [Remainder]string message)
        {
            var channel = Context.Client.GetGuild(guildid).GetTextChannel(channelid);
            await channel.SendMessageAsync(message);
        }



        [Command("eval")]
        [RunMode(RunMode.Parallel)]
        public Task EvalCodeAsync([Remainder] string script) => EvalService.EvaluateAsync(Context, Database, script);

    }
}
