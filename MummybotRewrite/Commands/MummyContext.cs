using Discord;
using Qmmands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

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

        public MummyContext(DiscordSocketClient client, IUserMessage msg)
        {
            Client = client;
            Guild = (msg.Channel as SocketGuildChannel)?.Guild;
            GuildChannel = msg.Channel as SocketGuildChannel;
            Channel = msg.Channel as SocketTextChannel;
            User = msg.Author as SocketGuildUser;
            Message = msg as SocketUserMessage;
        }
    }
}
