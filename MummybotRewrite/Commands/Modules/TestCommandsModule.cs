using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes.Checks;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
	public class TestCommandModule : MummyModule
    {
        [Command("test"),RequireOwner]
        public async Task TagAsync(string key)
        {
        }

        
    }
}