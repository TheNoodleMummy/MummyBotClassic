using Mummybot.Attributes.Checks;
using Mummybot.Enums;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireOwner]
	public class TestCommandModule : MummyModule
    {
       public CommandService Commands { get; set; }

        [Command("test")]
		public async Task Test()
        {
            foreach (var item in Commands.GetAllCommands())
            {
                foreach (var param in item.Parameters)
                {
                    if (param.IsMultiple)
                    {
                        Console.WriteLine(item.Name);
                    }

                }
            }
        }
       
    }
}