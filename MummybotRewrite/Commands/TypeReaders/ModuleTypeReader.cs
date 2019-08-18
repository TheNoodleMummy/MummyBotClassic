using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    class ModuleTypeReader : MummyTypeParser<Module>
    {
        public override async ValueTask<TypeParserResult<Module>> ParseAsync(Parameter parameter, string value, MummyContext context, IServiceProvider provider)
        {
            CommandService commandService = provider.GetRequiredService<CommandService>();
            var module = commandService.GetAllModules().FirstOrDefault(m => m.Name.Contains(value, StringComparison.CurrentCultureIgnoreCase));

            if (module == null)
                return new TypeParserResult<Module>("Could not find a module with that name");
            else
            {
                var result = await module.RunChecksAsync(context, provider);
                if (result.IsSuccessful)
                {
                    return new TypeParserResult<Module>(module);
                }
                else
                    return new TypeParserResult<Module>("you failed some checks and therefor i wont show you this module ;)");
            }
        }
    }
}
