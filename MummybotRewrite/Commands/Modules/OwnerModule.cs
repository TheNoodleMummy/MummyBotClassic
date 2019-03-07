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
        public EvalService evalService { get; set; }
        public TimerService TimerService { get; set; }


        [Command("timers")]
        public async Task Getimers()
        {
            var sb = new StringBuilder();
            foreach (var item in TimerService.Queue)
            {
                sb.AppendLine($"{item} in {item.When - DateTime.Now}");
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
                    await Messages.SendMessageAsync(Context, "this guild is not blacklisted");
            }

            [Command, Description("list all guilds that have been blacklist and why")]
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
        [RequireOwner]
        public Task EvalCodeAsync([Remainder] string script)
        => evalService.EvaluateAsync(Context, Database, script);

        //    try
        //    {
        //        foreach (var type in Assembly.GetEntryAssembly().GetTypes())
        //        {

        //        }
        //        var namespaces = Assembly.GetEntryAssembly().GetTypes().Select(x => x.Namespace).Distinct();
        //        var idk = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Distinct();
        //        namespaces = namespaces.Where(x => x != null);

        //        var options = ScriptOptions.Default;
        //        options = options.WithImports(string.Join(',', namespaces));
        //        options = options.WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location)));

        //        var result = await CSharpScript.EvaluateAsync(SanitizeCode(script), options, new Globals { Context = Context, Db = Database ,Messages = Messages});

        //        if (!(result is null))
        //        {
        //            await ReplyAsync($"```cs\n{result.ToString()}\n```").ConfigureAwait(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //         Logs.LogError("Eval Failed", Enums.LogSource.OwnerModule, ex);
        //    }

        //    string SanitizeCode(string s)
        //    {
        //        var cleanCode = s.Replace("```csharp", string.Empty).Replace("```cs", string.Empty).Replace("```", string.Empty);
        //        return Regex.Replace(cleanCode.Trim(), "^`|`$", string.Empty); //strip out the ` characters from the beginning and end of the string
        //   }




    }
}
