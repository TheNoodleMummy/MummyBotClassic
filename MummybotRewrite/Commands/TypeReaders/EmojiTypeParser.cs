using Discord;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    class EmojiTypeParser : TypeParser<Emoji>
    {
        public override Task<TypeParserResult<Emoji>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            Emoji emoji = null;
            try
            {
                emoji = new Emoji(value);
                return Task.FromResult(new TypeParserResult<Emoji>(emoji));
            }
            catch (Exception)
            {
                return Task.FromResult(new TypeParserResult<Emoji>("Failed to parse emote"));

            }  
        }
    }
}
