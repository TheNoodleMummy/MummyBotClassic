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

        [Command("test")]
		public async Task Test()
        {
            
        }      
    }
}