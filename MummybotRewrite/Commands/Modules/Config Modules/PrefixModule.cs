using Discord;
using Qmmands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("prefixes")]
    public class PrefixModule : MummyBase
    {

        [Command("add")]
        public async Task AddPrefixAsync([Remainder]string prefix)
        {

            if (GuildConfig.Prefixes.Any(p => p == prefix))
            {
                await ReplyAsync("This guild already has this prefix.");
            }
            else
            {
                GuildConfig.Prefixes.Add(prefix);
                await Context.Message.AddReactionAsync(new Emoji("✅"));

            }

        }

        [Command("remove")]
        public async Task RemovePrefixAsync([Remainder]string prefix)
        {

            if (GuildConfig.Prefixes.Any(p => p == prefix))
            {
                GuildConfig.Prefixes.Remove(prefix);
                await Context.Message.AddReactionAsync(new Emoji("✅"));

            }
            else
            {
                await ReplyAsync($"This guild doesnt have the ``{prefix}`` prefix.");

            }
        }

        [Command]
        public async Task ShowPrefixAsync()
        {
            var Prefixes = new List<string>();
            foreach (var item in GuildConfig.Prefixes)
            {
                Prefixes.Add($"``{item}`` ");
            }
            await ReplyAsync($"{string.Join(',', Prefixes)}");
        }



    }
}
