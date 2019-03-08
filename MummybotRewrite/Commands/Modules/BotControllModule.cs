using Discord;
using Mummybot.Attributes;
using Mummybot.Services;
using Qmmands;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Bot Control Module"), Description("Contains all command to control the bot")]
    public class BotControllModule : MummyBase
    {
        public PingService PingService { get; set; }

        [Command("reboot"), RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task Reboot([Remainder]string reason = null)
        {
            await (await Context.Client.GetApplicationInfoAsync()).Owner.SendMessageAsync($"im being rebooted by {Context.User.Username} with reason: {reason ?? "none"}");
            await Context.Client.SetStatusAsync(UserStatus.Offline);
            await Context.Client.SetGameAsync("being rebooted");
            _ = Task.Run(async () => await Context.Client.StopAsync());
            Environment.Exit(0);
        }


        [Command("kill"), RequireOwner]
        public async Task Kill([Remainder]string reason = null)
        {
            await (await Context.Client.GetApplicationInfoAsync()).Owner.SendMessageAsync($"im being shut down by {Context.User.Username} with reason: {reason ?? "none"}");
            await Context.Client.SetStatusAsync(UserStatus.Offline);

            Environment.Exit(1);
        }





        [Command("game")]
        [RequireOwner()]
        public Task Setgame(ActivityType activity, [Remainder()]string game = null)
       => Context.Client.SetGameAsync(game, type: activity);



        [Command("botname")]
        [RequireOwner()]
        public Task SetBotName([Remainder] string newname)
        => Context.Client.CurrentUser.ModifyAsync(x => x.Username = newname);


        [Command("avatar")]
        [RequireOwner()]
        public async Task SetBotavatar(string url = null)
        {
            url = url ?? Context.Message.Attachments.First().Url.ToString();
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(url);

            // returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;
            using (HttpWebResponse lxResponse = (HttpWebResponse)await lxRequest.GetResponseAsync())
            using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
            {
                Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                using (FileStream lxFS = new FileStream("configs/Avatar.png", FileMode.Create))
                {
                    lxFS.Write(lnByte, 0, lnByte.Length);
                }
            }

            await Task.Delay(1500);

            await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(@"configs/avatar.png"));
        }

        [Command("latency", "ping", "pong", "rtt")]
        [RunMode(RunMode.Parallel)]
        [Description("Returns the current estimated round-trip latency over WebSocket")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task Latency()
        {
            var emb = new EmbedBuilder();
            if (PingService.HasBeenoneHour)
                emb.AddField("Average Heartbeat in the last hour", PingService.PingInLastHour.Select(t => t.Value).Average().ToString("##.##"), false);

            emb.AddField("Average Heartbeat in my current life", PingService.AveragePing.Average().ToString("##.##"), false);

            var latency = Context.Client.Latency;
            var s = Stopwatch.StartNew();

            var m = await ReplyAsync($"heartbeat: {latency}ms, init: ---, rtt: ---", embed: emb.Build());
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
