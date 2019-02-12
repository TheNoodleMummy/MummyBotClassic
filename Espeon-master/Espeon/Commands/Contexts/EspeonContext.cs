﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net.Http;

namespace Espeon.Commands.Contexts
{
    public class EspeonContext : ICommandContext
    {
        public DiscordSocketClient Client { get; }
        public SocketGuild Guild { get; }
        public SocketGuildChannel GuildChannel { get; }
        public SocketTextChannel Channel { get; }
        public SocketGuildUser User { get; }
        public IUserMessage Message { get; }

        public HttpClient HttpClient;

        public EspeonContext(DiscordSocketClient client, IUserMessage msg, HttpClient httpClient)
        {
            Client = client;
            Guild = (msg.Channel as SocketGuildChannel)?.Guild;
            GuildChannel = msg.Channel as SocketGuildChannel;
            Channel = msg.Channel as SocketTextChannel;
            User = msg.Author as SocketGuildUser;
            Message = msg;
            HttpClient = httpClient;
        }

        IDiscordClient ICommandContext.Client => Client;
        IGuild ICommandContext.Guild => Guild;
        IMessageChannel ICommandContext.Channel => Channel;
        IUser ICommandContext.User => User;
        IUserMessage ICommandContext.Message => Message;
    }
}
