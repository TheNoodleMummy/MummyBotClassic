using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Discord.WebSocket;
using Discord;

namespace Mummybot.Commands.TypeReaders
{
    class IGuildTypeReader : TypeParser<IGuild>
    {
        public override Task<TypeParserResult<IGuild>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            var client = provider.GetService<DiscordSocketClient>();
            SocketGuild guild;
            if (ulong.TryParse(value, out ulong id))
                guild = client.GetGuild(id);
            else
            {
                guild = client.Guilds.FirstOrDefault(x => x.Name.ToLower() == value.ToLower());
            }


            if (guild == null)
                if (ulong.TryParse(value, out _))
                    return Task.FromResult(new TypeParserResult<IGuild>("Could not find a guildwith that id"));
                else
                    return Task.FromResult(new TypeParserResult<IGuild>("Could not find a guild with that name"));

            else
            {

                return Task.FromResult(new TypeParserResult<IGuild>(guild));
            }
        }

      
    }
    
}
