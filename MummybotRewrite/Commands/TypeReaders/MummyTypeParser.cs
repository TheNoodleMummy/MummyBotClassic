using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mummybot.Attributes;

namespace Mummybot.Commands.TypeReaders
{
    [DontOverride]
    public abstract class MummyTypeParser<T> : TypeParser<T>
    {
        public override ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, CommandContext context, IServiceProvider provider)
        => ParseAsync(parameter, value, (MummyContext)context, provider);

        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, MummyContext context, IServiceProvider provider);
    }
}
