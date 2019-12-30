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
    [RequireVoiceTrolls]
    public class VoiceTrollsModule : MummyModule
    {
        public MusicService _musicService;
        private readonly Random _rnd;

        public VoiceTrollsModule(MusicService musicService, Random rnd)
        {
            _musicService = musicService;
            _rnd = rnd;
        }

        [Command("ded","yourdedfluf"), RunMode(RunMode.Parallel)]
        public Task yourdedflufbuttAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\yourdedflulfbutt.mp3");

        [Command("stopscreaming"), RunMode(RunMode.Parallel)]
        public Task StopScreamingAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\stopscreaming.mp3");

        [Command("fuckingnigga"), RunMode(RunMode.Parallel),RequireOffensive]
        public Task FuckingNiggasAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\fucking niggas.mp3");

        [Command("booty"), RunMode(RunMode.Parallel)]
        public Task ItsYourDutyToPleaseThatBootyAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\booty.mp3");

        [Command("niggawhat", "what"), RunMode(RunMode.Parallel),RequireOffensive]
        public Task NiggaWhatAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\niggawhat.mp3");


        [Command("hefuckedup","hfu"),RunMode(RunMode.Parallel)]
        public Task hefuckedupAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\hefuckedup.mp3");

        [Command("bitchpls"), RunMode(RunMode.Parallel)]
        public Task bitchplsspaceAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\bitchplsspace.mp3");


        [Command("alah"), RequireOffensive, RunMode(RunMode.Parallel)]
        public Task alahAsync()
           => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\boom.mp3");

        [Command("blowen"), RunMode(RunMode.Parallel)]
        public Task BlowenAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\blowenopstraat.mp3");

        [Command("byekevin"), RunMode(RunMode.Parallel)]
        public Task ByeKevinAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\byekevin.mp3");

        [Command("cunt"), RunMode(RunMode.Parallel)]
        public Task CuntAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\cunt.mp3");

        [Command("cuntfree"), RunMode(RunMode.Parallel)]
        public Task CuntFreeAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\cuntfree.mp3");

        [Command("deeznuts"), RunMode(RunMode.Parallel)]
        public Task DeezNutsAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\deeznuts.mp3");

        [Command("dieoma"), RunMode(RunMode.Parallel)]
        public Task DieOmaAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\dieoma.mp3");

        [Command("getout"), RunMode(RunMode.Parallel)]
        public Task GetOutAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\getout.mp3");

        [Command("gg"), RunMode(RunMode.Parallel)]
        public Task GGAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\GG.mp3");

        [Command("ididit"), RunMode(RunMode.Parallel)]
        public Task IDidItAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\ididit.mp3");

        [Command("kbai"), RunMode(RunMode.Parallel)]
        public Task KBaiAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\kbai.mp3");

        [Command("kevincunt"), RunMode(RunMode.Parallel)]
        public Task KevinCuntAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\kevinstopbeingacunt" + _rnd.Next(3) + ".mp3");

        [Command("kickthebass"), RunMode(RunMode.Parallel)]
        public Task KickTheBassAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\Kickthebass.mp3");

        [Command("legalshit"), RunMode(RunMode.Parallel)]
        public Task LegalShitAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\legaleshit.mp3");

        [Command("letitgo"), RunMode(RunMode.Parallel)]
        public Task LetItGoAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\letitgo.mp3");

        [Command("noot"), RunMode(RunMode.Parallel)]
        public Task NootAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\noot.mp3");

        [Command("princess"), RunMode(RunMode.Parallel)]
        public Task PrettyPrincessAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\princess.mp3");

        [Command("pussyrape"), RunMode(RunMode.Parallel)]
        public Task PussyRapeAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\pussyrape.mp3");

        [Command("shia"), RunMode(RunMode.Parallel)]
        public Task ShiaAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\shia.mp3");

        [Command("stopmate"), RunMode(RunMode.Parallel)]
        public Task StopMateAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\stopdoingthatmate.mp3");

        [Command("sumbitch"), RunMode(RunMode.Parallel)]
        public Task SumBitchAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\sumbitch.mp3");

        [Command("wall"), RunMode(RunMode.Parallel)]
        public Task TrumpWallAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\trumpwall.mp3");

        [Command("tryme"), RunMode(RunMode.Parallel)]
        public Task TryMeBitchAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\trymebitch.mp3");

        [Command("wthc"), RunMode(RunMode.Parallel)]
        public Task WhoCaresAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\whothehellcares.mp3");

        [Command("Shu-ta", "Shuta"), RunMode(RunMode.Parallel)]
        public Task tmpAsync()
            => _musicService.PlayTroll(Context.Guild.Id, (Context.User as IVoiceState)?.VoiceChannel, @"..\yafc.mp3");

    }
}
