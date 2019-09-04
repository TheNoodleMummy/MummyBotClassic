using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class PingService : BaseService
    {
        public override Task InitialiseAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();
            Pings.Add(client.Latency);
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

        private Timer timer = new Timer(Callback, null, TimeSpan.FromHours(1), TimeSpan.FromHours(0));
        private static bool BeenHour = false;
        private Dictionary<DateTime, int> HourlyPings = new Dictionary<DateTime, int>();
        private List<int> Pings = new List<int>();

        public static void Callback(object state)
        {
            BeenHour = true;
        }

        public double HourPing
        {
            get
            {
                if (BeenHour)
                    return HourlyPings.Select(x => x.Value).Average();
                else
                    return 0;
            }
        }
        public double TotalPing
        {
            get
            {
                    return Pings.Average();                
            }
        }
    }
}
