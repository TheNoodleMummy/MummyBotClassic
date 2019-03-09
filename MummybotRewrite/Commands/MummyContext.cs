using Discord;
using Discord.WebSocket;
using Qmmands;
using System.Net.Http;

namespace Mummybot.Commands
{
    public class MummyContext : ICommandContext
    {
        public DiscordSocketClient Client { get; }
        public SocketGuild Guild { get; }
        public SocketGuildChannel GuildChannel { get; }
        public SocketTextChannel Channel { get; }
        public SocketGuildUser User { get; }
        public SocketUserMessage Message { get; }
        public HttpClient HttpClient { get; }

        public MummyContext(DiscordSocketClient client, IUserMessage msg,HttpClient httpClient)
        {
            HttpClient = httpClient;
            Client = client;
            Guild = (msg.Channel as SocketGuildChannel)?.Guild;
            GuildChannel = msg.Channel as SocketGuildChannel;
            Channel = msg.Channel as SocketTextChannel;
            User = msg.Author as SocketGuildUser;
            Message = msg as SocketUserMessage;
        }
    }
}
