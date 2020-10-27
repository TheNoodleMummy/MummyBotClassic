using Discord;
using Mummybot.Enums;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("nick", "nickname")]
    public class NicknameModsModule : MummyModule
    {
        [Group("prefix")]
        public class nicknameModsPrefix : MummyModule
        {
            [Command("add"), RunMode(RunMode.Parallel)]
            public async Task AddPrefix(string prefix)
            {
                foreach (var role in Context.Guild.Roles.Where(role => Context.Guild.EveryoneRole.Id != role.Id && role.Id != 188477343382110208))//mummybot role
                {
                    var newname = $"{prefix} {role.Name}";
                    LogService.LogInformation($"setting role name {newname}", LogSource.Commands, Context.GuildId);
                    await role.ModifyAsync(x => x.Name = newname);
                }
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }

            [Command("remove"), RunMode(RunMode.Parallel)]
            public async Task RemovePrefix(string prefix)
            {
                foreach (var role in Context.Guild.Roles.Where(role => Context.Guild.EveryoneRole.Id != role.Id && role.Id != 188477343382110208))//mummybot role
                {
                    var newname = role.Name.Remove(0, prefix.Length);
                    LogService.LogInformation($"setting role name {newname}", LogSource.Commands, Context.GuildId);
                    await role.ModifyAsync(x => x.Name = newname);
                }
                await Context.Message.AddReactionAsync(new Emoji("✅"));

            }


        }
    }
}
