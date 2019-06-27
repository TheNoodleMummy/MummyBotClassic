using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    public class EmptyCriterion<T> : ICriterion<T>
    {
        public Task<bool> JudgeAsync(MummyContext sourceContext, T parameter)
            => Task.FromResult(true);
    }
}
