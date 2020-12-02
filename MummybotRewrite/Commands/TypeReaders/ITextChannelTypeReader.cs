using Discord;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    public class ITextChannelTypeReader : MummyTypeParser<ITextChannel>
    {
        public override ValueTask<TypeParserResult<ITextChannel>> ParseAsync(Parameter parameter, string value, MummyContext ctx)
        {
            if (value.Length > 3 && value[0] == '<' && value[1] == '#' && value[^ 1] == '>' &&
               ulong.TryParse(value[2..^ 1], out var id) || ulong.TryParse(value, out id))
            {
                var chan = ctx.Guild.TextChannels.FirstOrDefault(c => c.Id == id) as ITextChannel;
                if (chan is null)
                    return TypeParserResult<ITextChannel>.Failed($"Could not find channel with id: {id}");
                else
                    return TypeParserResult<ITextChannel>.Successful(chan);
            }
            else
            {
                var chan = ctx.Guild.TextChannels.FirstOrDefault(c => c.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase));
                if (chan is null)
                    return TypeParserResult<ITextChannel>. Failed($"Could not find channel with name: {value}");
                else
                    return TypeParserResult<ITextChannel>.Successful(chan);
            }
        }
    }
}
