using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mummybot.Commands.TypeReaders
{
    public class CommandTypeReader : TypeParser<Command>
    {  

        public override Task<TypeParserResult<Command>> ParseAsync(Parameter param, string value, ICommandContext context, IServiceProvider provider)
        {

            var commands = provider.GetService<CommandService>().GetAllCommands().Where(c => c.Name.ToLower().Contains(value.ToLower()));

            if (commands.Count() > 1)
                return Task.FromResult(new TypeParserResult<Command>("Found multiple matches please refine your search queue"));
            else if (commands == null || commands.Count() == 0)
                return Task.FromResult(new TypeParserResult<Command>("Could not find a command with that name"));
            else

                return Task.FromResult(new TypeParserResult<Command>(commands.FirstOrDefault()));
        }

        //public override async Task<TypeParserResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        //{
        //    var commands = services.GetService<CommandService>().Commands.Where(c => c.Name.ToLower().Contains(input.ToLower()));

        //    if (commands.Count() > 1)
        //        return TypeReaderResult.FromError(CommandError.MultipleMatches, "Found Multiple matches please refine your search queue");
        //    else if (commands == null || commands.Count() == 0)
        //        return TypeReaderResult.FromError(CommandError.ObjectNotFound, "Could'nt find any command, please redefine your search queue");
        //    else

        //        return TypeReaderResult.FromSuccess(commands.FirstOrDefault());
        //}
    }
}