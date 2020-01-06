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
        [Command("inspect")]
        public async Task inspectid(ulong id)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds((long)(id >> 22)+ 1420070400000);
            var workerid = (id & 0x3E0000) >> 17;
            var processid = (id & 0x1F000) >> 12;
            var increment = id & 0xFFF;

            await ReplyAsync($"{id} was created on: {time}, with workerid: {workerid}, processid: {processid}, and increment: {increment}");

        }
        
    }
}