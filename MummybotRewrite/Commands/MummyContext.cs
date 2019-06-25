using Discord;
using Discord.WebSocket;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands
{
    public class MummyContext : CommandContext
    {
        public SocketGuildUser User { get; }
        public SocketTextChannel Channel { get; }
        public DiscordSocketClient Client { get; }
        public IUserMessage Message { get; }

        public SocketGuild Guild => User.Guild;
        public HttpClient HTTP { get; }
        public IServiceProvider ServiceProvider { get; }
        public string PrefixUsed { get; }

        public bool IsEdit { get; set; }

        internal static async Task<MummyContext> CreateAsync(DiscordSocketClient client, SocketUserMessage message, bool isEdit, string prefix)
        {
            throw new NotImplementedException();
        }
    }
}
