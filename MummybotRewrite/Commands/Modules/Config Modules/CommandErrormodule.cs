using Discord;
using Discord.Addons.Interactive;
using Mummybot.Attributes;
using Qmmands;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class CommandErrorModule : MummyBase
    {
        public InteractiveService InteractiveService { get; set; }

        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Command("commanderror")]
        [RunMode(RunMode.Parallel)]
        public async Task Enablecommanderrors()
        {
            if (GuildConfig.AdvancedCommandErrors)
            {
                var msg = await ReplyAsync("Are you sure you wane to disable command errors?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.AdvancedCommandErrors = false;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
            else
            {
                var msg = await ReplyAsync("Are you sure you wane to enable the command errors?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.AdvancedCommandErrors = true;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
        }
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Command("unknowncommand")]
        [RunMode(RunMode.Parallel)]
        public async Task Enableunknown()
        {
            if (GuildConfig.UnknownCommands)
            {
                var msg = await ReplyAsync("Are you sure you wane to disable unknown command errors?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UnknownCommands = false;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
            else
            {
                var msg = await ReplyAsync("Are you sure you wane to enable the unknown command errors?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UnknownCommands = true;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
        }
    }
}
