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

        [Command("inspect")]
        public async Task inspectid(ulong id)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds((long)(id >> 22) + 1420070400000);
            var workerid = (id & 0x3E0000) >> 17;
            var processid = (id & 0x1F000) >> 12;
            var increment = id & 0xFFF;

            await ReplyAsync($"{id} was created on: {time}, with workerid: {workerid}, processid: {processid}, and increment: {increment}");
        }


        [Command("ping")]
        [Description("get the bots heartbeat ping, time it takes to send a message, time to mofify this message")]
        public async Task PingAsync()
        {
            var emb = new EmbedBuilder();
            if (PingService.HourPing != 0)
            {
                emb.AddField("My average gateway Ping in the last hour", PingService.HourPing.ToString("##.##") + "ms", true);
            }
            if (PingService.TotalPing != 0)
            {
                emb.AddField("My total average gateway ping", PingService.TotalPing.ToString("##.##") + "ms", true);
            }

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
