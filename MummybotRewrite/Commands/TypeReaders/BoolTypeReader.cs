using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    public class BoolTypeReader : MummyTypeParser<bool>
    {
        public override ValueTask<TypeParserResult<bool>> ParseAsync(Parameter parameter, string value, MummyContext context)
        {
            if (value.Equals("on", StringComparison.CurrentCultureIgnoreCase))
                return TypeParserResult<bool>.Successful(true);
            else if (value.Equals("off", StringComparison.CurrentCultureIgnoreCase))
                return TypeParserResult<bool>.Successful(false);
            else
                return TypeParserResult<bool>.Failed($"could not parse {parameter.Name} as on/off");
        }

    }
}
