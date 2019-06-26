﻿using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mummybot;
using Mummybot.Attributes;
using Mummybot.Database;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace MummyBot
{
    internal class Heart
    {
        static void Main(string[] args)
            => new Heart().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly?.GetTypes().Where(t => t.GetCustomAttributes(true).Any(x => x is ServiceAttribute));
            var services = new ServiceCollection()
                .AddServices(types)
                .AddDbContext<GuildStore>(ServiceLifetime.Transient)
                .AddDbContext<TokenStore>(ServiceLifetime.Transient)
                 .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                 {
                     ExclusiveBulkDelete = true,
                     AlwaysDownloadUsers = true,
                     LogLevel = LogSeverity.Verbose,
                     MessageCacheSize = 100
                 }))
                 .AddSingleton(new CommandService(new CommandServiceConfiguration()
                 {
                     StringComparison = StringComparison.CurrentCultureIgnoreCase
                 }))
                 .AddSingleton<Random>()
                 .AddSingleton<HttpClient>()
                .BuildServiceProvider();

            services.GetRequiredService<MessageService>();

            using (var scope = services.CreateScope())
            {
                var tokenstore = services.GetRequiredService<TokenStore>();
                var guildstore = services.GetRequiredService<GuildStore>();
                await tokenstore.Database.MigrateAsync();
                await guildstore.Database.MigrateAsync();
                await tokenstore.SaveChangesAsync();
                await guildstore.SaveChangesAsync();
            }

                var mummybot = new BotStartup(services);
            await mummybot.StartAsync();
        }
    }
}