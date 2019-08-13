using Casino.Common;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;
using Mummybot.Commands.TypeReaders;
using Mummybot.Database;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot
{
    public class BotStartup
    {
        private readonly DiscordSocketClient DiscordClient;
        private readonly IServiceProvider Services;
        private readonly CommandService CommandService;
        private readonly TaskQueue taskQueue;
        private IEnumerable<Type> Types;


        public BotStartup(IServiceProvider services)
        {
            Services = services;
            DiscordClient = services.GetRequiredService<DiscordSocketClient>();
            CommandService = services.GetRequiredService<CommandService>();
            taskQueue = services.GetRequiredService<TaskQueue>();

            CommandService.AddModules(services.GetRequiredService<Assembly>());
            CommandService.AddTypeParser(parser: new UserTypeparser<SocketGuildUser>());

            taskQueue.OnError += (ex) => Task.Run(() => services.GetRequiredService<LogService>().LogError(string.Empty,Enums.LogSource.TaskQueue,ex));

            DiscordClient.Log += services.GetRequiredService<LogService>().LogEventAsync;
        }

        public async Task StartAsync(IEnumerable<Type> types)
        {
            Types = types;
            using (var tokenstore = Services.GetRequiredService<TokenStore>())
            {
#if DEBUG
                await DiscordClient.LoginAsync(TokenType.Bot, tokenstore.Tokens.FirstOrDefault(t=>t.BotName.Equals("dummybot",StringComparison.CurrentCultureIgnoreCase)).BotToken);
#else
                await DiscordClient.LoginAsync(TokenType.Bot, tokenstore.Tokens.FirstOrDefault(t=>t.BotName.Equals("mummybot",StringComparison.CurrentCultureIgnoreCase)).BotToken);
#endif
            }
            await DiscordClient.StartAsync();

            DiscordClient.Ready += DiscordClient_ReadyAsync;

            await Task.Delay(-1);
        }

        private async Task DiscordClient_ReadyAsync()
        {
            await Services.RunInitialisersAsync(Types);
            DiscordClient.Ready -= DiscordClient_ReadyAsync;
        }
    }
}
