using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mummybot.Commands.TypeReaders
{
    public class ModuleTypeReader : TypeParser<Module>
    {
        public override Task<TypeParserResult<Module>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            CommandService commandService = provider.GetService<CommandService>();
            var module = commandService.GetAllModules().FirstOrDefault(m => m.Name.ToLower().Contains(value.ToLower()));
            if (module == null)
                return Task.FromResult(new TypeParserResult<Module>("Could not find a module with that name"));
            else
                return Task.FromResult(new TypeParserResult<Module>(module));

        }

        //public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        //{
        //    CommandService commandService = services.GetService<CommandService>();
        //    var module = commandService.Modules.FirstOrDefault(m => m.Name.ToLower().Contains(input.ToLower()));
        //    if (module == null)
        //        return TypeReaderResult.FromError(CommandError.ObjectNotFound, "Could not find a module with that name");
        //    else
        //        return TypeReaderResult.FromSuccess(module);

        //}
    }
}