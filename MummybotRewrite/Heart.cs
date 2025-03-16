using Discord;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mummybot;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Enums;
using Mummybot.Exceptions;
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
                     GatewayIntents = GatewayIntents.All,
                     AlwaysDownloadUsers = true,
                     LogLevel = LogSeverity.Info,
                     MessageCacheSize = 100
                 }))
                 .AddSingleton<InteractiveService>()
                 .AddSingleton<Random>()
                 .AddSingleton(new CommandService(new CommandServiceConfiguration()
                 {
                     StringComparison = StringComparison.CurrentCultureIgnoreCase,
                     CooldownBucketKeyGenerator = CoolDownBucketGenerator
                 })
                 .AddTypeParsers(assembly))
                 .AddSingleton<HttpClient>()
                 .BuildServiceProvider();

            services.GetRequiredService<MessageService>();

           // using (var tokenstore = services.GetRequiredService<TokenStore>())
            using (var guildstore = services.GetRequiredService<GuildStore>())
            {
                //await tokenstore.Database.MigrateAsync();
                await guildstore.Database.MigrateAsync();

                //await tokenstore.SaveChangesAsync();
                await guildstore.SaveChangesAsync();
            }
            var mummybot = new BotStartup(services);
            await mummybot.StartAsync(types);
        }

        public object CoolDownBucketGenerator(object bucketType, CommandContext context)
        {
            if (!(context is MummyContext ctx))
                throw new InvalidContextException(context.GetType());

            if (bucketType is CooldownBucketType CBT)
                return CBT switch
                {
                    CooldownBucketType.Guilds => (object)ctx.GuildId,
                    CooldownBucketType.User => ctx.UserId,
                    CooldownBucketType.Channels => ctx.ChannelId,
                    CooldownBucketType.Global => ctx.Command,
                    _ => throw new InvalidOperationException("got unexpected cooldownbuckettype"),
                };
            throw new InvalidOperationException($"cooldownbuckettype failed to parse as {typeof(CooldownBucketType)}");
        }
    }
}