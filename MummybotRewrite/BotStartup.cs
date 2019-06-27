using Discord;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;
using Mummybot.Database;
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
    class BotStartup
    {
        private DiscordSocketClient DiscordClient;
        private IServiceProvider Services;
        private CommandService CommandService;
        public BotStartup(IServiceProvider services)
        {
            Services = services;
            DiscordClient = services.GetRequiredService<DiscordSocketClient>();
            CommandService = services.GetRequiredService<CommandService>();
            CommandService.AddModules(services.GetRequiredService<Assembly>());
            DiscordClient.Log += services.GetRequiredService<LogService>().LogEventAsync;
        }

        public async Task StartAsync()
        {

            using (var tokenstore = Services.GetRequiredService<TokenStore>())
            {
#if DEBUG
                await DiscordClient.LoginAsync(TokenType.Bot, tokenstore.Tokens.FirstOrDefault(t=>t.BotName.Equals("dummybot",StringComparison.CurrentCultureIgnoreCase)).BotToken);
#else
                await DiscordClient.LoginAsync(TokenType.Bot, tokenstore.Tokens.FirstOrDefault(t=>t.BotName.Equals("mummybot",StringComparison.CurrentCultureIgnoreCase)).BotToken);
#endif
            }
            await DiscordClient.StartAsync();
            await Task.Delay(-1);
        }
    }
}
