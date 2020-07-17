using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    class RandomModule :MummyModule
    {
        public Random rnd { get; set; }

        [Command("choose")]
        public async Task TagAsync(params string[] options)
        {
            var one = rnd.Next(options.Length);
            var two = rnd.Next(options.Length);
            var tree = rnd.Next(options.Length);
            var choise = (int)Math.Round(one + two + tree + 0.0);
            await ReplyAsync(options[choise]);
        }
    }
}
