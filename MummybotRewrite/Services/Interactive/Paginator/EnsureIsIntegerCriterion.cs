using System.Threading.Tasks;
using Qmmands;
using Discord.WebSocket;

namespace Discord.Addons.Interactive
{
    internal class EnsureIsIntegerCriterion : ICriterion<SocketMessage>
    {
        public Task<bool> JudgeAsync(ICommandContext sourceContext, SocketMessage parameter)
        {
            bool ok = int.TryParse(parameter.Content, out _);
            return Task.FromResult(ok);
        }
    }
}
