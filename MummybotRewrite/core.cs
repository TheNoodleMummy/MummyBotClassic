using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Victoria;

namespace Mummybot
{
    class Core
    {
        static void Main(string[] args)
        => new Core().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var assembly = Assembly.GetEntryAssembly();

            var serviceprovider = new ServiceCollection()
                .AddServices(assembly)
                 .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                 {
                     AlwaysDownloadUsers = true,
                     MessageCacheSize = 100,
                     LogLevel = LogSeverity.Verbose
                 }))
                 .AddSingleton(new CommandService(new CommandServiceConfiguration()
                 {
                     CaseSensitive = false
                 }))
            .AddSingleton<CommandService>()
            .AddSingleton(assembly)
            .AddSingleton(Process.GetCurrentProcess())
            .AddSingleton<Lavalink>()
            .AddSingleton<Random>()
            .AddSingleton(stopwatch)
            .BuildServiceProvider()
            .Inject(assembly)
            .RunInitialisers(assembly);

            var mummybot = new StartupService(serviceprovider);
            serviceprovider.Inject(mummybot);
            await mummybot.StartAsync();



        }
    }
}