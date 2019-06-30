using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
	public class TestCommandModule : MummyBase
    {
        [Command("test")]
		public Task Test()
        {
            Console.Write( string.Join("\n",TimeZoneInfo.GetSystemTimeZones()));
            return Task.CompletedTask;
        }
    }
}