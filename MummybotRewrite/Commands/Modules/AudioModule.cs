

using Mummybot.Services;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("audio Commands"), Description("holds some commands that will go away again after time or commands that are in test phase")]
    public class AudioModule : MummyBase
    {

        public AudioService Audio { get; set; }

        [Command("Join")]
        public Task Join()
           => Audio.ConnectAsync(Context.Guild.Id, Context.User, Context.Channel);

        [Command("Play")]
        public async Task PlayAsync([Remainder] string query)
    => await ReplyAsync(await Audio.PlayAsync(Context.Guild.Id, query));

        [Command("stop"),]
        public async Task StopAsync()
            => await ReplyAsync(await Audio.StopAsync(Context.Guild.Id));

        [Command("Leave")]
        public async Task Leave()
            => await ReplyAsync(await Audio.DisconnectAsync(Context.Guild.Id));

        [Command("Resume")]
        public async Task ResumeAsync()
           => await ReplyAsync(await Audio.ResumeAsync(Context.Guild.Id));

        [Command("Queue")]
        public Task Queue()
            => ReplyAsync(Audio.DisplayQueue(Context.Guild.Id));

        [Command("Seek")]
        public async Task SeekAsync(TimeSpan span)
            => await ReplyAsync(await Audio.SeekAsync(Context.Guild.Id, span));

        [Command("Skip")]
        public async Task SkipAsync()
            => await ReplyAsync(await Audio.SkipAsync(Context.Guild.Id, Context.User.Id));

        [Command("Volume")]
        public async Task Volume(int volume)
            =>await  ReplyAsync(await Audio.VolumeAsync(Context.Guild.Id, volume));
    }
}
