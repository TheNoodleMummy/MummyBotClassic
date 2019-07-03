using Discord;
using Mummybot.Attributes.Checks;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireVoiceChannel]
    public class MusicModule : MummyModule
    {
        private readonly AudioService _service;

        public MusicModule(AudioService service)
        {
            _service = service;
        }

        [Command("join"),RunMode(RunMode.Parallel)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState)?.VoiceChannel);
        }

        [Command("leave"), RunMode(RunMode.Parallel)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play"), RunMode(RunMode.Parallel)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}
