using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qmmands;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    public class EmptyCriterion<T> : ICriterion<T>
    {
        public Task<bool> JudgeAsync(ICommandContext sourceContext, T parameter)
            => Task.FromResult(true);
    }
}
