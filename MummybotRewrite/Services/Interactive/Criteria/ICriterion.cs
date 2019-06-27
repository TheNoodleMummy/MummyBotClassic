using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Mummybot.Commands;

namespace Discord.Addons.Interactive
{
    public interface ICriterion<in T>
    {
        Task<bool> JudgeAsync(MummyContext sourceContext, T parameter);
    }
}
