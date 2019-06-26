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

        public MummyContext(DiscordSocketClient client, IUserMessage message, HttpClient hTTP, IServiceProvider serviceProvider, string prefixUsed, bool isEdit)
        {
            Client = client;
            Message = message;
            HTTP = hTTP;
            ServiceProvider = serviceProvider;
            PrefixUsed = prefixUsed;
            IsEdit = isEdit;
        }

        public SocketGuildUser User { get; private set; }
        public SocketTextChannel Channel { get; private set; }
        public DiscordSocketClient Client { get; }
        public IUserMessage Message { get; }

        public SocketGuild Guild => User.Guild;
        public HttpClient HTTP { get; }
        public IServiceProvider ServiceProvider { get; }
        public string PrefixUsed { get; }

        public bool IsEdit { get; set; }

        internal static MummyContext Create(DiscordSocketClient client, IUserMessage message, HttpClient hTTP, IServiceProvider serviceProvider, string prefixUsed, bool isEdit)
        {
            return new MummyContext(client, message, hTTP, serviceProvider, prefixUsed, isEdit)
            {
                Channel = message.Channel as SocketTextChannel,
                User = message.Author as SocketGuildUser
            };
        }
       
    }
}
