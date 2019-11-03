﻿using System;
using Victoria;

namespace Mummybot.Services
{
    using Discord;
    using Discord.WebSocket;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.DependencyInjection;
    using Mummybot.Database;
    using Mummybot.Database.Entities;
    using Mummybot.Interfaces;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Victoria.Enums;
    using Victoria.EventArgs;
    using Victoria.Responses.Rest;

    public class MusicService : BaseService
    {

        public MusicService(LogService log)
        {
            LogService = log;
        }
        internal LavaNode lavaNode;
        

        private IServiceProvider Services;
        private readonly LogService LogService;


        public ConcurrentDictionary<ulong, MusicDetails> ConnectedChannels = new ConcurrentDictionary<ulong, MusicDetails>();

        public override async Task InitialiseAsync(IServiceProvider services)
        {
#if DEBUG
            lavaNode = services.GetRequiredService<LavaNode>();
            
            Services = services;
            
            lavaNode.OnWebSocketClosed += LavaClient_OnSocketClosed;
            lavaNode.OnTrackException += LavaClient_OnTrackException;
            lavaNode.OnTrackEnded += LavaClient_OnTrackFinished;
            lavaNode.OnTrackStuck += LavaClient_OnTrackStuck;

            lavaNode.OnLog += services.GetRequiredService<LogService>().LogLavalink;
            await lavaNode.ConnectAsync();
#endif
            return;
        }

        public async Task PlayTroll(ulong guildid, IVoiceChannel channel, string location)
        {
            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
            {
                TimeSpan position;
                IVoiceChannel voiceChannel;
                DefaultQueue<LavaTrack> queue;
                LavaTrack currenttrack;
                if (musicDetails.Player.PlayerState == PlayerState.Playing)
                {
                    position = musicDetails.Player.Track.Position;
                    voiceChannel = musicDetails.Player.VoiceChannel;
                    queue = musicDetails.Player.Queue;
                    currenttrack = musicDetails.Player.Track;

                    await musicDetails.Player.PauseAsync();
                    if (channel.Id == musicDetails.Player.VoiceChannel.Id)
                    {
                        var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
                        await musicDetails.Player.PlayAsync(track);
                        await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
                        await musicDetails.Player.PlayAsync(currenttrack);
                        await musicDetails.Player.SeekAsync(position);
                    }
                    else
                    {
                        await lavaNode.LeaveAsync(musicDetails.Player.VoiceChannel);

                        var player = await lavaNode.JoinAsync(channel);
                        var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
                        await player.PlayAsync(track);
                        await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
                        await lavaNode.LeaveAsync(channel);

                        musicDetails.Player = await lavaNode.JoinAsync(voiceChannel);
                        while (queue.TryDequeue(out var queuetrack))
                        {
                            musicDetails.Player.Queue.Enqueue(queuetrack);
                        }
                        await musicDetails.Player.PlayAsync(currenttrack);
                        await musicDetails.Player.SeekAsync(position);
                    }
                }
                else
                {
                    await lavaNode.LeaveAsync(musicDetails.Player.VoiceChannel);
                    var player = await lavaNode.JoinAsync(channel);
                    var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
                    await player.PlayAsync(track);
                    await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
                    await lavaNode.LeaveAsync(channel);
                }
            }
            else
            {
                var player = await lavaNode.JoinAsync(channel);
                var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
                await player.PlayAsync(track);
                await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
                await lavaNode.LeaveAsync(channel);
            }
        }

        internal async Task<PauseResult> PauseAsync(ulong guildId)
        {
            if (!(ConnectedChannels.TryGetValue(guildId, out var details)))
                 return new PauseResult() { IsSuccess = false, ErrorReason = "Im currently not connected to any voicechannel in this guild" };
            if (details.Player.PlayerState == PlayerState.Paused)
            {
                await details.Player.ResumeAsync();
                return new PauseResult() { IsSuccess = true, ErrorReason = "Succesfully Resumed"};
            }
            else
            {
                await details.Player.PauseAsync();
                return new PauseResult() { IsSuccess = true, ErrorReason = "Succesfully Paused" };
            }
                
        }

        public QueueResult GetQueue(ulong guildid)
        {
            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
                return new QueueResult() { IsSuccess=true, Queue= musicDetails.Player.Queue };
            return new QueueResult() { IsSuccess = false, ErrorReason = "Im currently not connected to any voicechannnel in this guild" };
        }


        public async Task<VolumeResult> SetVolumeAsync(ulong guildid, ushort volume)
        {
            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
            {
                if (volume <= 0 && volume >= 150)
                    return new VolumeResult() { IsSuccess = false, ErrorReason = "Volume must be between 0 and 150" };

                using var guildstore = Services.GetRequiredService<GuildStore>();
                var guild = await guildstore.GetOrCreateGuildAsync(guildid);
                guild.Volume = volume;
                guildstore.Update(guild);
                await guildstore.SaveChangesAsync();

                await musicDetails.Player.UpdateVolumeAsync(volume);
                return new VolumeResult() { IsSuccess = true, Volume = musicDetails.Player.Volume };
            }
            return new VolumeResult() { IsSuccess = false, ErrorReason = "Im Currently not connected to any voicechannnel in this guild" };
        }

        public async Task<PlayResult> PlayAsync(ulong guildid, string url)
        {
            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
            {
                var track = (await SearchTrackAsync(url)).Tracks.FirstOrDefault();

                if (musicDetails.Player.PlayerState == PlayerState.Playing)
                {
                    musicDetails.Player.Queue.Enqueue(track);
                    return new PlayResult() { PlayerWasPlaying = true, QueuePosition = musicDetails.Player.Queue.Items.Count(), Track = track };
                }
                else
                {
                    await musicDetails.Player.PlayAsync(track);
                    return new PlayResult() { PlayerWasPlaying = false, Track = track };
                }
            }
            return new PlayResult() { PlayerWasPlaying = false, WasConnected = false,ErrorReason= "Im Currently not connected to any voicechannnel in this guild" };
        }

        public async Task JoinAsync(IVoiceChannel channel = null)
        {
            if (channel is null)
                return;

            if (ConnectedChannels.TryGetValue(channel.GuildId, out MusicDetails musicDetails))
            {
                if (musicDetails.Player.VoiceChannel != null)
                {
                    await lavaNode.MoveAsync(channel);

                }
            }
            else
            {
                var player = await lavaNode.JoinAsync(channel);
                using var guildstore = Services.GetRequiredService<GuildStore>();
                var guild = await guildstore.GetOrCreateGuildAsync(channel.GuildId);
                await player.UpdateVolumeAsync((ushort)guild.Volume);
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
                await lavaNode.LeaveAsync(musicDetails.Player.VoiceChannel);
                ConnectedChannels.TryRemove(guildid, out var _);
            }
        }

        public Task<SearchResponse> SearchYoutubeAsync(string querry)
            => lavaNode.SearchYouTubeAsync(querry);

        public Task<SearchResponse> SearchSoundCloudAsync(string querry)
            => lavaNode.SearchSoundCloudAsync(querry);

        public Task<SearchResponse> SearchTrackAsync(string querry)
            => lavaNode.SearchAsync(querry);

        public Task<SearchResponse> SearchPlaylists(string querry)
            => lavaNode.SearchAsync(querry);

        private Task LavaClient_OnTrackStuck(TrackStuckEventArgs eventargs)
        {
            LogService.LogWarning($"player for {eventargs.Player.VoiceChannel.GuildId} stuck at {eventargs.Threshold}sec.", Enums.LogSource.Victoria,eventargs.Player.VoiceChannel.Guild.Id);
            return Task.CompletedTask;
        }

        private async Task LavaClient_OnTrackFinished(TrackEndedEventArgs eventargs)
        {
            if (!eventargs.Reason.ShouldPlayNext())
                return;

            if (!eventargs.Player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                if (eventargs.Player.TextChannel is null)
                    return;
                else
                    await eventargs.Player.TextChannel?.SendMessageAsync($"There are no more items left in queue.");
                return;
            }
            LogService.LogInformation($"playing next item in queue {nextTrack.Title}");
            await eventargs.Player.PlayAsync(nextTrack);
            if (eventargs.Player.TextChannel is null)
                return;
            else
                await eventargs.Player.TextChannel?.SendMessageAsync($"Now playing: {nextTrack.Title}");
        }

        private Task LavaClient_OnTrackException(TrackExceptionEventArgs eventargs)
        {
            LogService.LogError($"Player {eventargs.Player.VoiceChannel.GuildId} {eventargs.ErrorMessage} for {eventargs.Track.Title}", Enums.LogSource.Victoria,eventargs.Player.VoiceChannel.Guild.Id);
            return Task.CompletedTask;
        }

        private Task LavaClient_OnSocketClosed(WebSocketClosedEventArgs eventArgs)
        {
            LogService.LogError($"LavaNode Disconnected: {eventArgs.Reason} by {(eventArgs.ByRemote?"Remote":"Local")}",Enums.LogSource.LavaLink);
            return Task.CompletedTask;
        }
    }

    public class MusicDetails
    {
        public ulong GuildID { get; set; }

        public LavaPlayer Player { get; set; }
    }

    public class PlayResult : IMummyResult
    {
        public bool PlayerWasPlaying { get; set; }
        public int QueuePosition { get; set; }
        public LavaTrack Track { get; set; }

        public bool WasConnected { get; set; } = true;

        public bool IsSuccess { get; set; }
        public string ErrorReason { get; set; }
        public Exception Exception { get; set; }
    }

    public class VolumeResult : IMummyResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorReason { get; set; }

        public int Volume { get; set; }
        public Exception Exception { get; set; }
    }
    public class QueueResult : IMummyResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorReason { get; set; }
        public Exception Exception { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public DefaultQueue<LavaTrack> Queue { get; set; }
    }

    public class PauseResult : IMummyResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorReason { get; set; }
        public Exception Exception { get; set; }
    }
}
