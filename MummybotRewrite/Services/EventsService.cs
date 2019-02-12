using Qmmands;
using Discord.WebSocket;
using Mummybot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using System.Linq;
using System.Diagnostics;

namespace Mummybot.Services
{
    [Service(typeof(EventsService))]
    public class EventsService
    {
        [Inject]
        public DiscordSocketClient Client { get; set; }
        [Inject]
        public CommandService Commands { get; set; }
        [Inject]
        public LogService Logs { get; set; }
        [Inject]
        public MessagesService Message { get; set; }
        [Inject]
        public StarBoardService StarBoardService { get; set; }
        [Inject]
        public DBService DBService { get; set; }
        [Inject]
        public BirthdayService BirthdayService { get; set; }
        [Inject]
        public Lavalink Lava { get; set; }
        [Inject]
        public AudioService AudioService { get; set; }
        [Inject]
        public ReminderService ReminderService { get; set; }
        [Inject]
        public GuildService GuildService { get; set; }
        [Inject]
        public Stopwatch Stopwatch { get; set; }

        public void HookEvents()
        {
            Client.Log += Logs.LogEventAsync;
            Lava.Log += Logs.LogLavalink;
            
            Client.ReactionAdded += StarBoardService.OnReactionAddedAsync;
            Client.ReactionRemoved += StarBoardService.Client_ReactionRemoved;
            Client.Ready += Client_Ready;
            DBService.Hooklog();

            Client.MessageReceived += Message.HandleMessageAsync;
            Client.MessageUpdated += (_, msg, __) => Message.HandleMessageAsync(msg);
            
        }

        private async Task Client_Ready()
        {
            
            var node = await Lava.AddNodeAsync(Client, new Configuration
            {
#if DEBUG
                Authorization = "youshallnotpass",
                Host = "127.0.0.1",
                Port = 2333,
                ReconnectAttempts = -1,
                SelfDeaf = false,
                Severity = Discord.LogSeverity.Verbose,
                ReconnectInterval = TimeSpan.FromSeconds(5)



#else
                Authorization = "youshallnotpass",
                Host = "127.0.0.1",
                Port = 79,
                ReconnectAttempts = -1,
                SelfDeaf = false,
                Severity = Discord.LogSeverity.Verbose,
                ReconnectInterval = TimeSpan.FromSeconds(5)
                
               
#endif

            });
            var guilds = DBService.GetAllGuilds();
            foreach (var guild in Client.Guilds)
            {
                if (guilds.Any(x => x.GuildID == guild.Id)) continue;
                await GuildService.NewGuildAsync(guild.Id);
            }
            AudioService.Initialize(node);
            BirthdayService.CheckBDays(null);
            await ReminderService.InitializeAsync();
            Stopwatch.Stop();
            Logs.LogInformation($"Took {Stopwatch.ElapsedMilliseconds / 1000}second from start to ready.",Enums.LogSource.eventService);
        }
    }

}
