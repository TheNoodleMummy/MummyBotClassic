using Discord;
using Humanizer;
using Mummybot.Attributes.Checks;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria.Entities;

namespace Mummybot.Commands.Modules
{
    [RequireVoiceChannel]
    public class MusicModule : MummyModule
    {
        private readonly MusicService _musicService;

        public MusicModule(MusicService musicService, LogService logService)
        {
            _musicService = musicService;
            LogService = logService;
        }

        [Command("join")]
        public async Task Join()
        {
            await _musicService.JoinAsync((Context.User as IVoiceState)?.VoiceChannel);
            await Context.Message.AddOkAsync();
        }

        [Command("leave")]
        public async Task LeaveAsync()
        {
            await _musicService.LeaveAsync(Context.Guild.Id);
            await Context.Message.AddOkAsync();
        }

        [Command("volume")]
        public async Task SetVolumeAsync(int volume)
        {
            var result = await _musicService.SetVolumeAsync(Context.Guild.Id, volume);
            if (result.IsSuccess)
            {
                await ReplyAsync($"Changed Volume to {result.Volume}");
            }
            else
            {
                await ReplyAsync(result.ErrorReason);
            }
        }

        [Command("queue")]
        public async Task GetQueueAsync()
        {
            var result = _musicService.GetQueue(Context.Guild.Id);
            if (result.IsSuccess)
            {
                var sb = new StringBuilder();
                sb.AppendLine("```");
                var i = 1;
                foreach (LavaTrack item in result.Queue.Items.Take(10))
                {
                    sb.Append(i).Append(" [").Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Length.Humanize()).Append("](").Append(item.Uri).AppendLine(")");
                }
                sb.AppendLine().AppendLine("```");
                await ReplyAsync(embed:new EmbedBuilder().WithDescription( sb.ToString()));
            }
            else
            {
                await ReplyAsync(result.ErrorReason);
            }
        }

        [Command("play")]
        public async Task PlayAsync([Remainder]string url)
        {
            var playResult = await _musicService.PlayAsync(Context.Guild.Id, url);
            if (playResult.PlayerWasPlaying)
            {
                await ReplyAsync($"Added {playResult.Track.Title} To queue position: {playResult.QueuePosition}");
            }
            else if (!playResult.PlayerWasPlaying)
            {
                //now playing ....
            }
            else if (!playResult.WasConnected)
                await ReplyAsync("im not connected to voice");
        }

        [Command("ytsearch")]
        public async Task SearchYTAsync([Remainder]string querry)
        {
            var results = await _musicService.SearchYoutubeAsync(querry);
            switch (results.LoadType)
            {
                case LoadType.TrackLoaded:
                    await ReplyAsync($"found one track {results.Tracks.FirstOrDefault()}");
                    break;
                case LoadType.PlaylistLoaded:
                    var sb = new StringBuilder();
                    var emb = new EmbedBuilder();
                    foreach (var item in results.Tracks.Take(5))
                    {
                        sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Length.Humanize()).Append("](").Append(item.Uri).AppendLine(")");
                    }
                    emb.WithDescription(sb.ToString());
                    await ReplyAsync(embed: emb);
                    break;
                case LoadType.SearchResult:
                    sb = new StringBuilder();
                    emb = new EmbedBuilder();
                    foreach (var item in results.Tracks.Take(5))
                    {
                        sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Length.Humanize()).Append("](").Append(item.Uri).AppendLine(")");
                    }
                    emb.WithDescription(sb.ToString());
                    await ReplyAsync(embed: emb);
                    break;
                case LoadType.NoMatches:
                    await ReplyAsync($"couldnt find anything on Youtube for querry: {querry}");
                    break;
                case LoadType.LoadFailed:
                    await ReplyAsync("Something went wrong and i wa sunable to get any results from Youtube");
                    break;
            }
        }

        [Command("SCsearch")]
        public async Task SearchSCAsync([Remainder]string querry)
        {
            var results = await _musicService.SearchSoundCloudAsync(querry);
            switch (results.LoadType)
            {
                case LoadType.TrackLoaded:
                    await ReplyAsync($"found one track {results.Tracks.FirstOrDefault()}");
                    break;
                case LoadType.PlaylistLoaded:
                    var sb = new StringBuilder();
                    var emb = new EmbedBuilder();
                    foreach (var item in results.Tracks.Take(5))
                    {
                        sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Length.Humanize()).Append("](").Append(item.Uri).AppendLine(")");
                    }
                    emb.WithDescription(sb.ToString());
                    await ReplyAsync(embed: emb);
                    break;
                case LoadType.SearchResult:
                    sb = new StringBuilder();
                    emb = new EmbedBuilder();
                    foreach (var item in results.Tracks.Take(5))
                    {
                        sb.Append('[').Append(item.Title).Append(" by ").Append(item.Author).Append(" - ").Append(item.Length.Humanize()).Append("](").Append(item.Uri).AppendLine(")");
                    }
                    emb.WithDescription(sb.ToString());
                    await ReplyAsync(embed: emb);
                    break;
                case LoadType.NoMatches:
                    await ReplyAsync($"couldnt find anything on SoundCloud for querry: {querry}");
                    break;
                case LoadType.LoadFailed:
                    await ReplyAsync("Something went wrong and i was unable to get any results from SoundCloud");
                    break;
            }
        }
    }
}
