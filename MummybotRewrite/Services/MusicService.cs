using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mummybot.Services
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Audio;

    public class AudioService : BaseService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public AudioService(LogService logService)
        {
            LogService = logService;
        }

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
                LogService.LogInformation($"disconnected from voice for {guild.Name}", Enums.LogSource.MusicService);
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path,bool isurl = false)
        {
            // Your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path)&&!isurl)
            {
                LogService.LogError($"File @{path} does not exist", Enums.LogSource.MessagesService);
                await channel.SendMessageAsync("Something went wrong trying to paly this");
                return;
            }
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
                using (var ffmpeg = CreateProcess(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }
            }
        }

        private Process CreateProcess(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"\sounds\FFMPEG\ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
