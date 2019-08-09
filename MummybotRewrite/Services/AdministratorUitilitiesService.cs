using Casino.Common;
using Discord;
using Discord.WebSocket;
using Mummybot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    class AdministratorUitilitiesService : BaseService
    {
        private readonly TaskQueue TaskQueue;
        private readonly SnowFlakeGeneratorService SnowFlakeGenerator;
        public List<ScheduledTask<Voice>> MutedUsers = new List<ScheduledTask<Voice>>();
        public AdministratorUitilitiesService(TaskQueue taskQueue, SnowFlakeGeneratorService snowFlakeGenerator)
        {
            TaskQueue = taskQueue;
            SnowFlakeGenerator = snowFlakeGenerator;
        }

        public async Task VoiceMute(MummyContext ctx, SocketGuildUser user, TimeSpan time)
        {
            IRole role = ctx.Guild.Roles.FirstOrDefault(r => r.Name.Equals("muted", StringComparison.InvariantCultureIgnoreCase));
            if (role is null)
                role = await ctx.Guild.CreateRoleAsync("voice muted", new Discord.GuildPermissions(speak: false));

            user.AddRoleAsync(role);
            var id = SnowFlakeGenerator.NextLong();

            var callback = new Voice() { Role = role, User = user, Id = id };
            var task = TaskQueue.ScheduleTask(callback, time, VoiceMuteCallback, id);
            MutedUsers.Add(task);

        }

        public async Task CancelMuteAsync(SocketGuildUser user)
        {
            var mute = MutedUsers.FirstOrDefault(t => t.State.User.Id == user.Id);
            mute.Cancel();
            await mute.ToExecute(mute.State);
        }

        public async Task VoiceMuteCallback(Voice callbackobject)
        {
            await callbackobject.User.RemoveRoleAsync(callbackobject.Role);
            var mute = MutedUsers.FirstOrDefault(t => t.State.Id == callbackobject.Id);
            MutedUsers.Remove(mute);
        }

        public class Voice
        {
            public SocketGuildUser User { get; set; }
            public IRole Role { get; set; }
            public ulong Id { get; set; }
        }
    }
}
