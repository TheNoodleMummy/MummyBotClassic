using Discord;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mummybot;
using Mummybot.Database;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Victoria;

namespace MummyBot
{
    internal class Heart
    {
        private static void Main(string[] args)
            => new Heart().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly?.GetTypes()
                .Where(x => typeof(BaseService).IsAssignableFrom(x) && !x.IsAbstract).ToArray();

            var services = new ServiceCollection()
                .AddServices(types)
                .AddSingleton(assembly)
                .AddDbContext<GuildStore>(ServiceLifetime.Transient)
                .AddDbContext<TokenStore>(ServiceLifetime.Transient)
                 .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                 {
                     ExclusiveBulkDelete = true,
                     AlwaysDownloadUsers = true,
                     LogLevel = LogSeverity.Info,
                     MessageCacheSize = 100
                 }))
                 .AddSingleton<InteractiveService>()
                 .AddSingleton<LavaSocketClient>()
                 .AddSingleton<LavaRestClient>()
                 .AddSingleton<Random>()
                 .AddSingleton(new CommandService(new CommandServiceConfiguration()
                 {
                     StringComparison = StringComparison.CurrentCultureIgnoreCase
                 })
                 .AddTypeParsers(assembly))
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
            await mummybot.StartAsync(types);
        }
    }
}