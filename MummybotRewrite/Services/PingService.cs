using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class PingService 
    {

        private Dictionary<DateTime, int> HourlyPings = new Dictionary<DateTime, int>();
        private List<int> Pings = new List<int>();

        public double HourPing => HourlyPings.Select(x=>x.Value).Average();
        public double TotalPing => Pings.Average();

        public PingService(DiscordSocketClient client)
        {
            client.LatencyUpdated += (_, newping) =>
            {
                Pings.Add(newping);
                HourlyPings.Add(DateTime.UtcNow, newping);
                var toremove = HourlyPings.Where(x => x.Key <= DateTime.UtcNow);
                foreach (var item in toremove)
                {
                    HourlyPings.Remove(item.Key);
                }
                return Task.CompletedTask;
            };
        }
    }
}
