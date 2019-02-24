using Discord;
using Discord.WebSocket;
using Mummybot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    [Service(typeof(PingService))]
    public class PingService
    {
        [Inject]
        private readonly DiscordSocketClient client;

        public Timer Hour = new Timer(HourGoneBy, null, TimeSpan.FromHours(1), TimeSpan.Zero);
        private static void HourGoneBy(object state) => HasBeenHour = true;

        public bool HasBeenoneHour { get { return HasBeenHour; } }

        public static bool HasBeenHour = false;



        public Dictionary<DateTime, int> PingInLastHour { get; set; } = new Dictionary<DateTime, int>();
        public int HourPingUpdates { get; set; } = 0;


        public List<int> AveragePing { get; set; } = new List<int>();
        public int PingUpdates { get; set; } = 0;



        [Initialiser]
        public void InstallPingService()
        {
            client.LatencyUpdated += Client_LatencyUpdated;
            client.LatencyUpdated += async (int Older, int Newer) =>
            {
                await LatencyUpdatedAsync(client, Older, Newer);
            };

        }
        public async Task LatencyUpdatedAsync(DiscordSocketClient Client, int Older, int Newer)
        {
            if (Client == null) return;

            UserStatus Status = (Client.ConnectionState == ConnectionState.Disconnected || Newer > 150) ? UserStatus.DoNotDisturb
                : (Client.ConnectionState == ConnectionState.Connecting || Newer > 120) ? UserStatus.Idle
                : (Client.ConnectionState == ConnectionState.Connected || Newer < 120) ? UserStatus.Online : UserStatus.AFK;
            if (Client.CurrentUser?.Status != Status)
                await Client.SetStatusAsync(Status);
        }



        internal Task Client_LatencyUpdated(int lastping, int newping)
        {
            HourPingUpdates++;
            PingInLastHour.Add(DateTime.Now, newping);


            foreach (var item in PingInLastHour.Where(x => DateTime.Now.AddHours(-1) > x.Key))
            {
                PingInLastHour.Remove(item.Key);
                HourPingUpdates--;
            }


            PingUpdates++;
            AveragePing.Add(newping);

            return Task.CompletedTask;
        }
    }
}