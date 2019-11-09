using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Enums;
using Mummybot.Services;
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

        public MummyContext(DiscordSocketClient client, IUserMessage message, HttpClient hTTP, IServiceProvider services, string prefixUsed, bool isEdit) : base(services)
        {
            Client = client;
            Message = message;
            HTTP = hTTP;
            PrefixUsed = prefixUsed;
            IsEdit = isEdit;
            LogService = services.GetRequiredService<LogService>();
        }       

        public SocketGuildUser User { get; private set; }
        public SocketTextChannel Channel { get; private set; }
        public DiscordSocketClient Client { get; }
        public IUserMessage Message { get; }

        public SocketGuild Guild => User.Guild;
        public HttpClient HTTP { get; }
        public LogService LogService {get;}
        public string PrefixUsed { get; }

        public bool IsEdit { get; set; }


        public ulong ChannelId => Channel.Id;
        public ulong UserId => User.Id;
        public ulong GuildId => Guild.Id;
        
        internal void LogDebug(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
       => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Debug, source.ToString(), Message, exception, Guild));

        internal void LogWarning(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Warning, source.ToString(), Message, exception, Guild));

        internal void LogVerbose(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Verbose, source.ToString(), Message, exception, Guild));

        internal void LogCritical(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Critical, source.ToString(), Message, exception, Guild));

        internal void LogError(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Error, source.ToString(), Message, exception, Guild));

        internal void LogInformation(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogService.LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Info, source.ToString(), Message, exception, Guild));
    
        internal static MummyContext Create(DiscordSocketClient client, IUserMessage message, HttpClient hTTP,IServiceProvider services, string prefixUsed, bool isEdit)
        {
            return new MummyContext(client, message, hTTP,services, prefixUsed, isEdit)
            {
                Channel = message.Channel as SocketTextChannel,
                User = message.Author as SocketGuildUser
            };
        }       
    }
}
