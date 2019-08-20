using Discord;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class BotUtilModule : MummyModule
    {
        public PingService PingService { get; set; }

        [Command("ping")]
        [Description("get the bots heartbeatping,time it takes to send a message,time to mofify this message")]
        public async Task PingAsync()
        {
            var emb = new EmbedBuilder()
                .AddField("My average gateway Ping in the last hour", PingService.HourPing.ToString("##.##"), true)
                .AddField("My total average ping", PingService.TotalPing.ToString("##.##"), true);

            var latency = Context.Client.Latency;
            var s = Stopwatch.StartNew();

            var m = await ReplyAsync($"heartbeat: {latency}ms, init: ---, rtt: ---", embed: emb);
            var init = s.ElapsedMilliseconds;
            await m.ModifyAsync(x =>
            {
                x.Content = $"heartbeat: {latency}ms, init: {init}ms, rtt: Calculating";
                x.Embed = emb.Build();
            });
            s.Stop();
            await m.ModifyAsync(x =>
            {
                x.Content = $"heartbeat: {latency}ms, init: {init}ms, rtt: {s.ElapsedMilliseconds}ms";
                x.Embed = emb.Build();
            });

           
        }
    }
}
