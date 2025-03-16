//using Discord;
//using Discord.WebSocket;
//using Humanizer;
//using Mummybot.Attributes.Checks;
//using Mummybot.Commands.TypeReaders;
//using Mummybot.Extentions;
//using Mummybot.Services;
//using Qmmands;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Victoria;
//using Victoria.Enums;

//namespace Mummybot.Commands.Modules
//{
//    [RequireVoiceChannel]
//    [RequireMusicAttribute]
//    public class MusicModule : MummyModule
//    {
//        private readonly MusicService _musicService;

//        public MusicModule(MusicService musicService, LogService logService)
//        {
//            _musicService = musicService;
//            LogService = logService;
//        }

//        [Command("join")]
//        [Description("makes to bot join your voicechannel")]
//        public async Task Join(ITextChannel textchannel = null)
//        {
//            await _musicService.JoinAsync((Context.User as IVoiceState)?.VoiceChannel, textchannel);
//            await Context.Message.AddOkAsync();
//        }

//        [Command("join")]
//        [Description("makes to bot join your voicechannel")]
//        public async Task Join([OverrideTypeParser(typeof(BoolTypeReader))] bool reportornot = false)
//        {
//            await _musicService.JoinAsync((Context.User as IVoiceState)?.VoiceChannel, Context.Channel);
//            await Context.Message.AddOkAsync();
//        }


//        [Command("leave")]
//        [Description("makes to bot leave your voicechannel")]
//        public async Task LeaveAsync()
//        {
//            await _musicService.LeaveAsync(Context.Guild.Id,Context.User.VoiceChannel);
//            await Context.Message.AddOkAsync();
//        }

//        [Command("volume")]
//        [Description("sets the bots volume")]
//        public async Task SetVolumeAsync([Description("volule to set")] ushort volume)
//        {
//            var result = await _musicService.SetVolumeAsync(Context.Guild.Id, volume);
//            if (result.IsSuccess)
//            {
//                await ReplyAsync($"Changed Volume to {result.Volume}");
//            }
//            else
//            {
//                await ReplyAsync(result.ErrorReason);
//            }
//        }

//        [Command("queue")]
//        [Description("get the current queue for this guild")]
//        public async Task GetQueueAsync()
//        {
//            var result = _musicService.GetQueue(Context.Guild.Id);
//            if (result.IsSuccess)
//            {
//                var sb = new StringBuilder();
//                var i = 1;
//                if (result.Queue.Count == 0)
//                {
//                    await ReplyAsync("nothing in queue");
//                    return;
//                }
//                foreach (LavaTrack track in result.Queue?.Take(10))
//                {
//                    sb.Append(i).Append(" [").Append(track.Title).Append(" uploaded by ").Append(track.Author).Append(" - ").Append(track.Duration).Append("](").Append(track.Url).AppendLine(")");
//                }
//                await ReplyAsync(embed: new EmbedBuilder().WithDescription(sb.ToString()));
//            }
//            else
//            {
//                await ReplyAsync(result.ErrorReason);
//            }
//        }

//        [Command("play")]
//        [Description("play something? (the bot needs to join first)")]
//        public async Task PlayAsync([Description("a youtube/soundcloud link"), Remainder] string url)
//        {
//            var canrequestplaylists = GuildConfig.PlayListWhiteLists.Any(x => x.UserId == Context.UserId);

//            var playResult = await _musicService.PlayAsync(Context.Guild.Id, url, canrequestplaylists);
//            if (playResult.IsSuccess && !playResult.isPlayList)
//            {
//                if (playResult.PlayerWasPlaying)
//                {
//                    await ReplyAsync($"Added {playResult.Tracks.FirstOrDefault().Title} to the queue position: {playResult.QueuePosition}");
//                }
//                else if (!playResult.PlayerWasPlaying)
//                {
//                    await ReplyAsync($"now palying {playResult.Tracks.FirstOrDefault().Title}");
//                }
//                else
//                {
//                    await ReplyAsync($"{playResult.ErrorReason}");
//                }
//            }
//            else if (playResult.IsSuccess && playResult.isPlayList)
//            {
//                if (playResult.PlayerWasPlaying)
//                {
//                    await ReplyAsync($"Added {playResult.QueuePosition} tracks to the queue positions: {playResult.PlaylistInQueue.First}-{playResult.PlaylistInQueue.Last}");

//                }
//                else if (!playResult.PlayerWasPlaying)
//                {
//                    await ReplyAsync($"added {playResult.QueuePosition - 1} tracks to the queue positions: {playResult.PlaylistInQueue.First}-{playResult.PlaylistInQueue.Last}");
//                }
//            }
//            else if (!playResult.WasConnected)
//                await ReplyAsync("im not connected to voice");

//        }

//        [Command("pause")]
//        [Description("pause the current song")]
//        public async Task PauseAsync()
//        {
//            var pauseresult = await _musicService.PauseAsync(Context.GuildId);
//            if (pauseresult.IsSuccess)
//            {
//                await ReplyAsync(pauseresult.ErrorReason);
//                await Context.Message.AddOkAsync();
//            }
//            else
//                await Context.Message.AddNotOkAsync();
//        }

//        //[Command("ytsearch")]
//        //public async Task SearchYTAsync([Description("the querry to look for on youtube"), Remainder] string querry)
//        //{
//        //    var results = await _musicService.SearchYoutubeAsync(querry);
//        //    switch (results.LoadType)
//        //    {
//        //        case LoadType.TrackLoaded:
//        //            await ReplyAsync($"found one track {results.Tracks.FirstOrDefault()}");
//        //            break;
//        //        case LoadType.PlaylistLoaded:
//        //            var sb = new StringBuilder();
//        //            var emb = new EmbedBuilder();
//        //            foreach (var item in results.Tracks.Take(5))
//        //            {
//        //                sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Duration.Humanize()).Append("](").Append(item.Url).AppendLine(")");
//        //            }
//        //            emb.WithDescription(sb.ToString());
//        //            await ReplyAsync(embed: emb);
//        //            break;
//        //        case LoadType.SearchResult:
//        //            sb = new StringBuilder();
//        //            emb = new EmbedBuilder();
//        //            foreach (var item in results.Tracks.Take(5))
//        //            {
//        //                sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Duration.Humanize()).Append("](").Append(item.Url).AppendLine(")");
//        //            }
//        //            emb.WithDescription(sb.ToString());
//        //            await ReplyAsync(embed: emb);
//        //            break;
//        //        case LoadType.NoMatches:
//        //            await ReplyAsync($"couldnt find anything on Youtube for querry: {querry}");
//        //            break;
//        //        case LoadType.LoadFailed:
//        //            await ReplyAsync("Something went wrong and i was unable to get any results from Youtube");
//        //            break;
//        //    }
//        //}

//        //[Command("SCsearch")]
//        //public async Task SearchSCAsync([Description("the querry to look for on soundcloud"), Remainder] string querry)
//        //{
//        //    var results = await _musicService.SearchSoundCloudAsync(querry);
//        //    switch (results.LoadType)
//        //    {
//        //        case LoadType.TrackLoaded:
//        //            await ReplyAsync($"found one track {results.Tracks.FirstOrDefault()}");
//        //            break;
//        //        case LoadType.PlaylistLoaded:
//        //            var sb = new StringBuilder();
//        //            var emb = new EmbedBuilder();
//        //            foreach (var item in results.Tracks.Take(5))
//        //            {
//        //                sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Duration.Humanize()).Append("](").Append(item.Url).AppendLine(")");
//        //            }
//        //            emb.WithDescription(sb.ToString());
//        //            await ReplyAsync(embed: emb);
//        //            break;
//        //        case LoadType.SearchResult:
//        //            sb = new StringBuilder();
//        //            emb = new EmbedBuilder();
//        //            foreach (var item in results.Tracks.Take(5))
//        //            {
//        //                sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Duration.Humanize()).Append("](").Append(item.Url).AppendLine(")");
//        //            }
//        //            emb.WithDescription(sb.ToString());
//        //            await ReplyAsync(embed: emb);
//        //            break;
//        //        case LoadType.NoMatches:
//        //            await ReplyAsync($"couldnt find anything on SoundCloud for querry: {querry}");
//        //            break;
//        //        case LoadType.LoadFailed:
//        //            await ReplyAsync("Something went wrong and i was unable to get any results from SoundCloud");
//        //            break;
//        //    }
//        //}
//    }
//}
