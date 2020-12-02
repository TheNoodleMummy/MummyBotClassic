using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    class DifficultyTypeParser : MummyTypeParser<Difficulty>
    {
        public override ValueTask<TypeParserResult<Difficulty>> ParseAsync(Parameter parameter, string value, MummyContext context)
        {
            if (value.Equals("easy", StringComparison.InvariantCultureIgnoreCase))
            {
                return TypeParserResult<Difficulty>.Successful(Difficulty.Easy);
            }
            if (value.Equals("normal", StringComparison.InvariantCultureIgnoreCase))
            {
                return TypeParserResult<Difficulty>.Successful(Difficulty.Normal);
            }
            if (value.Equals("hard", StringComparison.InvariantCultureIgnoreCase))
            {
                return TypeParserResult<Difficulty>.Successful(Difficulty.Hard);
            }
            if (value.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
            {
                return TypeParserResult<Difficulty>.Successful(Difficulty.Custom);
            }
            return TypeParserResult<Difficulty>.Failed($"couldnt parse {value} to a valid Difficulty");
        }
    }
}
