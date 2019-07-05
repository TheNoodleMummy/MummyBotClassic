using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Victoria;

namespace Mummybot.Services
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Audio;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using Victoria.Entities;

    public class MusicService : BaseService
    {
        public LavaSocketClient LavaSocketClient { get; set; }
        public LavaRestClient LavaRestClient { get; set; }

        public ConcurrentDictionary<ulong, MusicDetails> ConnectedChannels = new ConcurrentDictionary<ulong, MusicDetails>();

        public override Task InitialiseAsync(IServiceProvider services)
        {
            LavaSocketClient = services.GetRequiredService<LavaSocketClient>();
            LavaSocketClient.StartAsync(services.GetRequiredService<DiscordSocketClient>(), new Configuration()
            {
                AutoDisconnect = false,
                SelfDeaf = false,
                LogSeverity = LogSeverity.Debug,
                ReconnectInterval = TimeSpan.FromSeconds(10),
                Host = "127.0.0.1",
                Port = 2333,
                Password = "youshallnotpass"
            });
            LavaRestClient = services.GetRequiredService<LavaRestClient>();

            LavaSocketClient.OnSocketClosed += LavaClient_OnSocketClosed;
            LavaSocketClient.OnTrackException += LavaClient_OnTrackException;
            LavaSocketClient.OnTrackFinished += LavaClient_OnTrackFinished;
            LavaSocketClient.OnTrackStuck += LavaClient_OnTrackStuck;

            LavaSocketClient.Log += services.GetRequiredService<LogService>().LogLavalink;
            return Task.CompletedTask;
        }

        public async Task<PlayResult> PlayAsync(ulong guildid,string url)
        {
            if (ConnectedChannels.TryGetValue(guildid,out var musicDetails))
            {
                var track = (await LavaRestClient.SearchTracksAsync(url)).Tracks.FirstOrDefault();

                if (musicDetails.Player.IsPlaying)
                {
                    musicDetails.Player.Queue.Enqueue(track);
                    return new PlayResult() { PlayerWasPlaying = true,QueuePosition = musicDetails.Player.Queue.Items.Count(),Track = track};
                }
                else
                {
                    await musicDetails.Player.PlayAsync(track);
                    return new PlayResult() { PlayerWasPlaying = false, Track = track };
                }
            }
            return new PlayResult() { PlayerWasPlaying = false,WasConnected = false};
        }

        public async Task JoinAsync(IVoiceChannel channel= null)
        {
            if (channel is null)
                return;

            if (ConnectedChannels.TryGetValue(channel.GuildId,out MusicDetails musicDetails))
            {
                if (musicDetails.Player.VoiceChannel != null)
                {
                    await LavaSocketClient.MoveChannelsAsync(channel);
                }
            }
            else
            {
                var player = await LavaSocketClient.ConnectAsync(channel);
                ConnectedChannels.TryAdd(channel.GuildId, new MusicDetails()
                {
                    Player = player,
                    GuildID = channel.GuildId
                });
            }
        }

        public async Task LeaveAsync(ulong guildid)
        {
            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
            {
                await LavaSocketClient.DisconnectAsync(musicDetails.Player.VoiceChannel);
                ConnectedChannels.TryRemove(guildid, out var music);
            }
        }

        public Task<SearchResult> SearchYoutubeAsync(string querry)
            => LavaRestClient.SearchYouTubeAsync(querry);

        public Task<SearchResult> SearchSoundCloudAsync(string querry)
            => LavaRestClient.SearchSoundcloudAsync(querry);

        public Task<SearchResult> SearchTrack(string querry)
            => LavaRestClient.SearchTracksAsync(querry);

        public Task<SearchResult> SearchPlaylists(string querry)
            => LavaRestClient.SearchTracksAsync(querry, true);

        private Task LavaClient_OnTrackStuck(LavaPlayer player, LavaTrack track, long threshold)
        {
            LogService.LogWarning($"player for {player.VoiceChannel.GuildId} stuck at {threshold}sec.",Enums.LogSource.Victoria);
            return Task.CompletedTask;
        }

        private async Task LavaClient_OnTrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext())
                return;

            if (!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                await player.TextChannel?.SendMessageAsync($"There are no more items left in queue.");
                return;
            }

            await player.PlayAsync(nextTrack);
            await player.TextChannel?.SendMessageAsync($"Finished playing: {track.Title}\nNow playing: {nextTrack.Title}");
        }

        private Task LavaClient_OnTrackException(LavaPlayer player, LavaTrack track, string error)
        {
            LogService.LogError($"Player {player.VoiceChannel.GuildId} {error} for {track.Title}", Enums.LogSource.Victoria);
            return Task.CompletedTask;
        }

        private Task LavaClient_OnSocketClosed(int errorcode, string Reason, bool ByRemote)
        {
            LogService.LogError($"LavaLinkNode Disconnected: {Reason}");
            return Task.CompletedTask;
        }
    }

    public class MusicDetails
    {
        public ulong GuildID { get; set; }

        public LavaPlayer Player { get; set; }
    }

    public class PlayResult
    {
        public bool PlayerWasPlaying { get; set; }
        public int QueuePosition { get; set; }
        public LavaTrack Track { get; set; }

        public bool WasConnected { get; set; } = true;
    }
}
