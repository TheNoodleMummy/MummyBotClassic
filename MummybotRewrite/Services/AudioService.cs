using Discord;
using Mummybot.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;
using Victoria.Entities.Enums;

namespace Mummybot.Services
{
    [Service(typeof(AudioService))]
    public sealed class AudioService
    {
        private LavaNode LavaNode;
        private readonly ConcurrentDictionary<ulong, (LavaTrack Track, List<ulong> Votes)> VoteSkip;
        private TimeSpan _defaultTimeout = TimeSpan.FromSeconds(10);
        public LogService Logs { get; set; }


        public AudioService(LogService logs)
        {
            Logs = logs;
            VoteSkip = new ConcurrentDictionary<ulong, (LavaTrack Track, List<ulong> Votes)>();
        }

        public void Initialize(LavaNode node)
        {
            LavaNode = node;
            node.TrackStuck += OnStuck;
            node.TrackFinished += OnFinished;
            node.PlayerUpdated += OnUpdated;
            node.TrackException += OnException;
        }

        //public async Task PlayTrollAsync(ulong guildid,ulong Voicechannelid,string trollpath, TimeSpan? timeout = null)
        //{
        //    var player = LavaNode.GetPlayer(guildid);

        //    var IsPlaying = player.CurrentTrack != null;
        //    player.Pause();
        //    var currentposition = player.CurrentTrack?. ?? TimeSpan.FromSeconds(0);
        //    var currenttrack = player?.CurrentTrack;

        //    var vchan = player.VoiceChannel;
        //    var tchan = player.TextChannel;


        //    var track = (await LavaNode.GetTracksAsync(trollpath)).Tracks.FirstOrDefault();


        //    TimeSpan? trolllenght = track.Length+TimeSpan.FromSeconds(2);
        //    player.Play(track);

        //    /*get when the troll is done or wait for timeout*/
        //    var eventTrigger = new TaskCompletionSource<bool>();
        //    async Task Handler(LavaPlayer Eplayer, LavaTrack Etrack,TrackReason trackReason)
        //    {
        //        await Task.Delay(TimeSpan.FromSeconds(2));
        //        eventTrigger.SetResult(true);
        //        await Task.CompletedTask;
        //    }
        //    LavaNode.Finished += Handler;
        //    timeout = timeout ?? trolllenght ?? _defaultTimeout;
        //    var trigger = eventTrigger.Task;
        //    var delay = Task.Delay(timeout.Value);
        //    var task = await Task.WhenAny(trigger, delay).ConfigureAwait(false);
        //    LavaNode.Finished -= Handler;
        //    /*troll should be done or timed out here*/

        //    if (IsPlaying)
        //    {
        //        player = await LavaNode.JoinAsync(vchan, tchan);
        //        player.Play(currenttrack);
        //        player.Pause();
        //        player.Seek(currentposition);
        //        player.Resume();
        //    }
        //}

        public async Task<string> PlayAsync(ulong guildId, string query)
        {
            var search = Uri.IsWellFormedUriString(query, UriKind.RelativeOrAbsolute)
                ? await LavaNode.GetTracksAsync(query)
                : await LavaNode.SearchYouTubeAsync(query);

            var track = search.Tracks.FirstOrDefault();
            var player = LavaNode.GetPlayer(guildId);
            if (player.CurrentTrack != null)
            {
                player.Queue.Enqueue(track);
                return $"**Enqueued:** {track.Title}";
            }

            await player.PlayAsync(track);
            return $"**Playing:** {track.Title}";
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var leave = await LavaNode.DisconnectAsync(guildId);
            return leave ? "Disconnected!" : "Can't leave when I'm not connected??";
        }

        public async Task<string> PauseAsync(ulong guildId)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                await player.PauseAsync();
                return $"**Paused:** {player.CurrentTrack.Title}";
            }
            catch
            {
                return "Not playing anything currently.";
            }
        }

        public async Task<string> ResumeAsync(ulong guildId)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                await player.PauseAsync();
                return $"**Resumed:** {player.CurrentTrack.Title}";
            }
            catch
            {
                return "Not playing anything currently.";
            }
        }

        public string DisplayQueue(ulong guildId)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                return string.Join("\n", player.Queue.Items.Select(x => $"=> {x.Title}")) ?? "Your queue is empty.";
            }
            catch
            {
                return "Your queue is empty.";
            }
        }

        public async Task<string> VolumeAsync(ulong guildId, int vol)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                await player.SetVolumeAsync(vol);
                return $"Volume has been set to {vol}.";
            }
            catch (ArgumentException arg)
            {
                return arg.Message;
            }
            catch
            {
                return "Not playing anything currently.";
            }
        }

        public async Task<string> SeekAsync(ulong guildId, TimeSpan span)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                await player.SeekAsync(span);
                return $"**Seeked:** {player.CurrentTrack.Title}";
            }
            catch
            {
                return "Not playing anything currently.";
            }
        }

        public async Task<string> SkipAsync(ulong guildId, ulong userId)
        {
            var player = LavaNode.GetPlayer(guildId);
            try
            {
                var users = (await player.VoiceChannel.GetUsersAsync().FlattenAsync()).Count(x => !x.IsBot);
                if (!VoteSkip.ContainsKey(guildId))
                    VoteSkip.TryAdd(guildId, (player.CurrentTrack, new List<ulong>()));
                VoteSkip.TryGetValue(guildId, out var skipInfo);

                if (!skipInfo.Votes.Contains(userId)) skipInfo.Votes.Add(userId);
                var perc = (int)Math.Round((double)(100 * skipInfo.Votes.Count) / users);
                if (perc <= 50) return "More votes needed.";
                VoteSkip.TryUpdate(guildId, skipInfo, skipInfo);
                await player.StopAsync();
                return $"**Skipped:** {player.CurrentTrack.Title}";
            }
            catch
            {
                return "Not playing anything currently.";
            }
        }

        public async Task ConnectAsync(ulong guildId, IVoiceState state, IMessageChannel channel)
        {
            if (state.VoiceChannel == null)
            {
                await channel.SendMessageAsync("You aren't connected to any voice channels.");
                return;
            }

            var player = await LavaNode.ConnectAsync(state.VoiceChannel, channel);
            await channel.SendMessageAsync($"Connected to {state.VoiceChannel}.");
        }

        public async Task<string> DisconnectAsync(ulong guildId)
            => await LavaNode.DisconnectAsync(guildId) ? "Disconnected." : "Not connected to any voice channels.";

        private async Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            if (player == null)
                return;
            player.Queue.TryDequeue(out var nextTrack);
            if (nextTrack == null)
            {
                await LavaNode.DisconnectAsync(player.VoiceChannel.GuildId);
                await player.TextChannel.SendMessageAsync("Queue Completed!");
                return;
            }

            await player.PlayAsync(nextTrack);
            await player.TextChannel.SendMessageAsync($"**Now Playing:** {track.Title}");
        }

        private async Task OnStuck(LavaPlayer player, LavaTrack track, long arg3)
        {
            player.Queue.Enqueue(track);
            Logs.LogCritical($"Track {track.Title} got stuck.", Enums.LogSource.AudioService);
            await player.TextChannel.SendMessageAsync(
                $"Track {track.Title} got stuck: {arg3}. Track has been requeued.");
        }

        private Task OnUpdated(LavaPlayer player, LavaTrack track, TimeSpan span)
        {
            Logs.LogInformation($"{span} {track?.Title}.");
            return Task.CompletedTask;
        }

        private async Task OnException(LavaPlayer player, LavaTrack track, string arg3)
        {
            player.Queue.Enqueue(track);
            Logs.LogCritical($"Track {track.Title} threw an exception: {arg3}", Enums.LogSource.AudioService, new Exception(arg3));
            await player.TextChannel.SendMessageAsync(
                $"Track {track.Title} threw an exception: {arg3}. Track has been requeued.");
        }
    }
}