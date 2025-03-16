//namespace Mummybot.Services
//{
//    using Discord;
//    using Discord.WebSocket;
//    using Microsoft.EntityFrameworkCore.ChangeTracking;
//    using Microsoft.Extensions.DependencyInjection;
//    using Mummybot.Database;
//    using Mummybot.Database.Entities;
//    using Mummybot.Interfaces;
//    using System;
//    using System.Collections.Concurrent;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using Victoria;
//    using Victoria.Rest;
//    using Victoria.Rest.Search;
//    using Victoria.WebSocket.EventArgs;

//    public class MusicService : BaseService
//    {

//        public MusicService(LogService log,IServiceProvider services, LavaNode<LavaPlayer<LavaTrack>, LavaTrack> lavaNode)
//        {
//            LogService = log;
//            _lavaNode = lavaNode;
//            _lavaNode.OnWebSocketClosed += LavaClient_OnSocketClosed;
//            _lavaNode.OnTrackException += LavaClient_OnTrackException;
//            _lavaNode.OnTrackEnd += LavaNode_OnTrackEnd; ;
//            _lavaNode.OnTrackStuck += LavaClient_OnTrackStuck;
//            Services = services; 
//        }
//        private readonly LavaNode<LavaPlayer<LavaTrack>, LavaTrack> _lavaNode;

//        private IServiceProvider Services;
//        private readonly LogService LogService;


//        public ConcurrentDictionary<ulong, MusicDetails> ConnectedChannels = new ConcurrentDictionary<ulong, MusicDetails>();

//        //public override async Task InitialiseAsync(IServiceProvider services)
//        //{


//        //    Services = services;

//        //    _lavaNode.OnWebSocketClosed += LavaClient_OnSocketClosed;
//        //    _lavaNode.OnTrackException += LavaClient_OnTrackException;
//        //    _lavaNode.OnTrackEnd += LavaNode_OnTrackEnd; ;
//        //    _lavaNode.OnTrackStuck += LavaClient_OnTrackStuck;


//        //    return;
//        //}


//        //public async Task PlayTroll(ulong guildid, IVoiceChannel channel, string location)
//        //{
//        //    if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
//        //    {
//        //        TimeSpan position;
//        //        IVoiceChannel voiceChannel;
//        //        LavaQueue<LavaTrack> queue;
//        //        LavaTrack currenttrack;
//        //        var player = lavaNode.GetPlayerAsync(guildid);
//        //        if (player != null)
//        //        {
//        //            position = musicDetails.Player.Track.Position;
//        //            voiceChannel = musicDetails.Player.VoiceChannel;
//        //            queue = musicDetails.Player.Queue;
//        //            currenttrack = musicDetails.Player.Track;

//        //            await musicDetails.Player.PauseAsync();
//        //            if (channel.Id == musicDetails.Player.VoiceChannel.Id)
//        //            {
//        //                var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
//        //                await musicDetails.Player.PlayAsync(track);
//        //                await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
//        //                await musicDetails.Player.PlayAsync(currenttrack);
//        //                await musicDetails.Player.SeekAsync(position);
//        //            }
//        //            else
//        //            {
//        //                await lavaNode.LeaveAsync(musicDetails.Player.VoiceChannel);

//        //                var player = await lavaNode.JoinAsync(channel);
//        //                var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
//        //                await player.PlayAsync(track);
//        //                await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
//        //                await lavaNode.LeaveAsync(channel);

//        //                musicDetails.Player = await lavaNode.JoinAsync(voiceChannel);
//        //                while (queue.TryDequeue(out var queuetrack))
//        //                {
//        //                    musicDetails.Player.Queue.Enqueue(queuetrack);
//        //                }
//        //                await musicDetails.Player.PlayAsync(currenttrack);
//        //                await musicDetails.Player.SeekAsync(position);
//        //            }
//        //        }
//        //        else
//        //        {
//        //            await lavaNode.LeaveAsync(musicDetails.Player);
//        //            var player = await lavaNode.JoinAsync(channel);
//        //            var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
//        //            await player.PlayAsync(track);
//        //            await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
//        //            await lavaNode.LeaveAsync(channel);
//        //        }
//        //    }
//        //    else
//        //    {
//        //        var player = await lavaNode.JoinAsync(channel);
//        //        var track = (await SearchTrackAsync(location)).Tracks.FirstOrDefault();
//        //        await player.PlayAsync(track);
//        //        await Task.Delay(track.Duration.Add(TimeSpan.FromSeconds(2)));
//        //        await lavaNode.LeaveAsync(channel);
//        //    }
//        //}

//        internal async Task<PauseResult> PauseAsync(ulong guildId)
//        {
//            if (!(ConnectedChannels.TryGetValue(guildId, out var details)))
//                return new PauseResult() { IsSuccess = false, ErrorReason = "Im currently not connected to any voicechannel in this guild" };
//            if (details.Player.IsPaused)
//            {
//                await details.Player.ResumeAsync(_lavaNode, details.Player.Track);
//                return new PauseResult() { IsSuccess = true, ErrorReason = "Succesfully Resumed" };
//            }
//            else
//            {
//                await details.Player.PauseAsync(_lavaNode);
//                return new PauseResult() { IsSuccess = true, ErrorReason = "Succesfully Paused" };
//            }

//        }

//        public QueueResult GetQueue(ulong guildid)
//        {
//            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
//                return new QueueResult() { IsSuccess = true, Queue = musicDetails.Player.GetQueue() };
//            return new QueueResult() { IsSuccess = false, ErrorReason = "Im currently not connected to any voicechannnel in this guild" };
//        }




//        public async Task<VolumeResult> SetVolumeAsync(ulong guildid, ushort volume)
//        {
//            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
//            {
//                if (volume <= 0 && volume >= 150)
//                {
//                    await (musicDetails.TextChannel?.SendMessageAsync("Volume must be between 0 and 150") ?? Task.CompletedTask);
//                    return new VolumeResult() { IsSuccess = false, ErrorReason = "Volume must be between 0 and 150" };
//                }

//                using var guildstore = Services.GetRequiredService<GuildStore>();
//                var guild = await guildstore.GetOrCreateGuildAsync(guildid);
//                guild.Volume = volume;
//                await guildstore.SaveChangesAsync();
//                await musicDetails.Player.SetVolumeAsync(_lavaNode, volume);
//                await (musicDetails.TextChannel?.SendMessageAsync($"Set volume to {volume}") ?? Task.CompletedTask);
//                return new VolumeResult() { IsSuccess = true, Volume = musicDetails.Player.Volume };
//            }
//            return new VolumeResult() { IsSuccess = false, ErrorReason = "Im Currently not connected to any voicechannnel in this guild" };
//        }

//        public async Task<PlayResult> PlayAsync(ulong guildid, string url, bool canplaylist)
//        {
//            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
//            {
//                var result = (await SearchTrackAsync(url));
//                if (result.Tracks.Count == 1)
//                {
//                    if (!musicDetails.Player.IsPaused)
//                    {
//                        musicDetails.Player.GetQueue().Enqueue(result.Tracks.FirstOrDefault());
//                        await (musicDetails.TextChannel?.SendMessageAsync($"added {result.Tracks.First().Title} to position {musicDetails.Player.GetQueue().Count}") ?? Task.CompletedTask);
//                        return new PlayResult() { IsSuccess = true, PlayerWasPlaying = true, QueuePosition = musicDetails.Player.GetQueue().Count, Tracks = (ICollection<LavaTrack>)result.Tracks };
//                    }
//                    else
//                    {
//                        await musicDetails.Player.PlayAsync(_lavaNode, result.Tracks.FirstOrDefault());
//                        await (musicDetails.TextChannel?.SendMessageAsync($"now playing {result.Tracks.First().Title} <{result.Tracks.First().Url}>") ?? Task.CompletedTask);
//                        return new PlayResult() { IsSuccess = true, PlayerWasPlaying = false, Tracks = (ICollection<LavaTrack>)result.Tracks };
//                    }
//                }
//                if (result.Tracks.Count > 1)
//                {
//                    if (canplaylist)
//                    {
//                        bool first = false;
//                        int firstposition = 0, lastposition = 0;

//                        if (!musicDetails.Player.IsPaused)
//                        {
//                            foreach (var track in result.Tracks)
//                            {
//                                musicDetails.Player.GetQueue().Enqueue(track);
//                                if (!first)
//                                {
//                                    firstposition = musicDetails.Player.GetQueue().Count;
//                                    first = true;
//                                }
//                            }
//                            lastposition = musicDetails.Player.GetQueue().Count;
//                            await (musicDetails.TextChannel?.SendMessageAsync($"added {result.Tracks.Count} to the queue postions {firstposition}-{lastposition}") ?? Task.CompletedTask);

//                            return new PlayResult()
//                            {
//                                IsSuccess = true,
//                                isPlayList = true,
//                                Tracks = (ICollection<LavaTrack>)result.Tracks,
//                                PlaylistInQueue = (firstposition, lastposition),
//                                QueuePosition = result.Tracks.Count,
//                                PlayerWasPlaying = true
//                            };
//                        }
//                        else
//                        {
//                            foreach (var track in result.Tracks)
//                            {
//                                musicDetails.Player.GetQueue().Enqueue(track);
//                                if (!first)
//                                {
//                                    firstposition = 1;
//                                    first = true;
//                                }
//                            }
//                            lastposition = musicDetails.Player.GetQueue().Count;
//                            musicDetails.Player.GetQueue().TryDequeue(out var qtrack);
//                            await musicDetails.Player.PlayAsync(_lavaNode, qtrack);
//                            await (musicDetails.TextChannel?.SendMessageAsync($"now playing {qtrack.Title} <{qtrack.Url}> \n and added {result.Tracks.Count} to queue.") ?? Task.CompletedTask);
//                            return new PlayResult()
//                            {
//                                IsSuccess = true,
//                                isPlayList = true,
//                                Tracks = (ICollection<LavaTrack>)result.Tracks,
//                                PlaylistInQueue = (firstposition, lastposition),
//                                QueuePosition = result.Tracks.Count
//                            };
//                        }
//                    }
//                    await (musicDetails.TextChannel?.SendMessageAsync("you arent whitelisted to request playlists") ?? Task.CompletedTask);
//                    return new PlayResult() { isPlayList = true, ErrorReason = "user is not whitelisted to request playlists" };
//                }
//                return new PlayResult() { IsSuccess = true, isPlayList = true, Tracks = (ICollection<LavaTrack>)result.Tracks };
//            }
//            return new PlayResult() { PlayerWasPlaying = false, WasConnected = false, ErrorReason = "Im Currently not connected to any voicechannnel in this guild" };
//        }

//        public async Task JoinAsync(IVoiceChannel channel, ITextChannel textchannel = null)
//        {
//            MusicDetails musicDetails = new MusicDetails();
//            if (channel is null)
//                return;

//            if (ConnectedChannels.TryGetValue(channel.GuildId, out musicDetails))
//            {
//                if (musicDetails.Player != null)
//                {
//                    musicDetails.Player = await _lavaNode.TryGetPlayerAsync(channel.GuildId);
//                }
//            }
//            else
//            {
//                LavaPlayer<LavaTrack> player;
//                if (textchannel is null)
//                    musicDetails.Player = await _lavaNode.JoinAsync(channel);
//                else
//                    musicDetails.Player = await _lavaNode.JoinAsync(channel);

//                using var guildstore = Services.GetRequiredService<GuildStore>();
//                var guild = await guildstore.GetOrCreateGuildAsync(channel.GuildId);
//                await musicDetails.Player.SetVolumeAsync(_lavaNode, guild.Volume);
//                await (textchannel?.SendMessageAsync($"joined {channel.Name} with volume set to {guild.Volume}") ?? Task.CompletedTask);
//                ConnectedChannels.TryAdd(channel.GuildId, musicDetails);
//            }
//        }

//        public async Task LeaveAsync(ulong guildid, IVoiceChannel channel)
//        {
//            if (ConnectedChannels.TryGetValue(guildid, out var musicDetails))
//            {
//                await _lavaNode.LeaveAsync(channel);
//                ConnectedChannels.TryRemove(guildid, out var _);
//            }
//        }

//        //public Task<SearchResponse> SearchYoutubeAsync(string querry)
//        //    => lavaNode.SearchYouTubeAsync(querry);

//        //public Task<SearchResponse> SearchSoundCloudAsync(string querry)
//        //    => lavaNode.SearchSoundCloudAsync(querry);

//        public Task<SearchResponse> SearchTrackAsync(string querry)
//            => _lavaNode.LoadTrackAsync(querry);

//        //public Task<SearchResponse> SearchPlaylists(string querry)
//        //    => lavaNode.SearchAsync(querry);

//        private Task LavaClient_OnTrackStuck(TrackStuckEventArg eventargs)
//        {
//            if (ConnectedChannels.TryGetValue(eventargs.GuildId, out MusicDetails musicDetails))
//            {
//                LogService.LogWarning($"player for {musicDetails.GuildID} stuck at {eventargs.Threshold}sec.", Enums.LogSource.Victoria, eventargs.GuildId);
//                return Task.CompletedTask;
//            }
//            return Task.CompletedTask;

//        }

//        private async Task LavaNode_OnTrackEnd(TrackEndEventArg eventargs)
//        {
//            MusicDetails musicDetails = new MusicDetails();
//            if (ConnectedChannels.TryGetValue(eventargs.GuildId, out musicDetails))
//            {
//                if (musicDetails.Player.GetQueue().TryDequeue(out var item1) || !(item1 is LavaTrack nextTrack1))
//                {
//                    await musicDetails.TextChannel?.SendMessageAsync("There are no more items left in queue.");
//                }
//                LogService.LogInformation($"playing {item1.Title}", Enums.LogSource.Victoria, musicDetails.GuildID);
//                await musicDetails.Player.PlayAsync(_lavaNode, item1);
//                if (musicDetails.TextChannel is null)
//                    return;
//                else
//                    await musicDetails.TextChannel?.SendMessageAsync($"Now playing: {item1.Title}");
//            }
//        }

//        private async Task LavaClient_OnTrackException(TrackExceptionEventArg eventargs)
//        {
//            LogService.LogInformation($"Player {eventargs.GuildId} {eventargs.Exception.Message} for {eventargs.Track.Title}", Enums.LogSource.Victoria, eventargs.GuildId);
//        }

//        private Task LavaClient_OnSocketClosed(WebSocketClosedEventArg eventargs)
//        {
//            LogService.LogError($"LavaNode Disconnected: {eventargs.Reason} by {(eventargs.ByRemote ? "Remote" : "Local")}", Enums.LogSource.LavaLink);
//            return Task.CompletedTask;
//        }
//    }

//    public class MusicDetails
//    {
//        public ulong GuildID { get; set; }

//        public LavaPlayer<LavaTrack> Player { get; set; }

//        public SocketTextChannel TextChannel { get; set; }

//    }

//    public class PlayResult : IMummyResult
//    {
//        public bool isPlayList { get; set; } = false;
//        public bool PlayerWasPlaying { get; set; }
//        public int QueuePosition { get; set; }
//        public (int First, int Last) PlaylistInQueue { get; set; }
//        public ICollection<LavaTrack> Tracks { get; set; } = new List<LavaTrack>();

//        public bool WasConnected { get; set; } = true;

//        public bool IsSuccess { get; set; }
//        public string ErrorReason { get; set; }
//        public Exception Exception { get; set; }
//    }

//    public class VolumeResult : IMummyResult
//    {
//        public bool IsSuccess { get; set; }
//        public string ErrorReason { get; set; }
//        public int Volume { get; set; }
//        public Exception Exception { get; set; }
//    }
//    public class QueueResult : IMummyResult
//    {
//        public bool IsSuccess { get; set; }
//        public string ErrorReason { get; set; }
//        public Exception Exception { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

//        public LavaQueue<LavaTrack> Queue { get; set; }
//    }

//    public class PauseResult : IMummyResult
//    {
//        public bool IsSuccess { get; set; }
//        public string ErrorReason { get; set; }
//        public Exception Exception { get; set; }
//    }
//}
