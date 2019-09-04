using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class PingService :BaseService
    {
        public override Task InitialiseAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();
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
            return Task.CompletedTask;
        }

        private Dictionary<DateTime, int> HourlyPings = new Dictionary<DateTime, int>();
        private List<int> Pings = new List<int>();

        public double HourPing => HourlyPings.Select(x=>x.Value).Average();
        public double TotalPing => Pings.Average();        
    }
}
