using Discord;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    class ITextChannelTypeReader : TypeParser<ITextChannel>
    {
        //todo make it boi
        public override ValueTask<TypeParserResult<ITextChannel>> ParseAsync(Parameter parameter, string value, CommandContext context, IServiceProvider provider)
        {
            var ctx = context as MummyContext;
            if (ulong.TryParse(value, out ulong id))
            {
                var chan = ctx.Guild.GetChannel(id) as ITextChannel;
                if (chan is null)
                    return TypeParserResult<ITextChannel>.Unsuccessful($"Could not find channel with id: {id}");
                else
                    return TypeParserResult<ITextChannel>.Successful(chan);
            }
            else
            {
                var chan = ctx.Guild.TextChannels.FirstOrDefault(c => c.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase));
                if (chan is null)
                    return TypeParserResult<ITextChannel>.Unsuccessful($"Could not find channel with name: {value}");
                else
                    return TypeParserResult<ITextChannel>.Successful(chan);
            }
        }
    }
}
