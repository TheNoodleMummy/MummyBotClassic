using Casino.Common;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class AdministratorUitilitiesService : BaseService
    {
        private readonly TaskQueue TaskQueue;
        private readonly SnowFlakeGeneratorService SnowFlakeGenerator;
        private readonly DiscordSocketClient DiscordClient;
        private readonly IServiceProvider ServiceProvider;
        private readonly LogService LogService;

        public AdministratorUitilitiesService(
            TaskQueue taskQueue,
            SnowFlakeGeneratorService snowFlakeGenerator,
            DiscordSocketClient discordclient,
            IServiceProvider serviceProvider,
            LogService logs)
        {
            TaskQueue = taskQueue;
            SnowFlakeGenerator = snowFlakeGenerator;
            DiscordClient = discordclient;
            ServiceProvider = serviceProvider;
            LogService = serviceProvider.GetRequiredService<LogService>();
        }

        public override async Task InitialiseAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();
            //getall current muted users and put them back in taskqueue/unmute them if time has passed
            using (var store = services.GetRequiredService<GuildStore>())
            {
                var guildconfigs = await store.GetAllGuildsAsync(x => x.VoiceMutedUsers);
                foreach (Guild guild in guildconfigs)
                {
                    var socketguild = client.GetGuild(guild.GuildID);
                    var vmus = guild.VoiceMutedUsers.Where(r => r.ExpiresAtUTC < DateTimeOffset.UtcNow).ToList();
                    LogService.LogInformation($"unmuting {vmus.Count} expired VoiceMutes.", LogSource.AdministratotUtilService,Guild:socketguild);
                    foreach (var vmu in vmus)
                    {
                        await VoiceMuteCallback(vmu);
                        guild.VoiceMutedUsers.Remove(vmu);
                    }

                    store.Update(guild);

                    foreach (var item in guild.VoiceMutedUsers)
                    {
                        TaskQueue.ScheduleTask(item, item.ExpiresAtUTC, VoiceMuteCallback);
                    }
                }
                await store.SaveChangesAsync();

            }
        }

        public async Task VoiceMute(MummyContext ctx, SocketGuildUser user, TimeSpan time)
        {
            var vmu = new VoiceMutedUser()
            {
                Id = SnowFlakeGenerator.NextLong(),
                ChannelID = ctx.Channel.Id,
                SetAtUTC = DateTimeOffset.UtcNow,
                ExpiresAtUTC = DateTimeOffset.UtcNow.Add(time),
                UserID = user.Id,
                GuildID = ctx.Guild.Id
            };

            await ctx.Channel.SendMessageAsync($"Muted {user.GetDisplayName()} for {time.Humanize()}");
            LogService.LogInformation($"Muted {user.GetDisplayName()} for {time.Humanize()}", Guild: ctx.Guild);

            using (var store = ServiceProvider.GetRequiredService<GuildStore>())
            {
                var config = await store.GetOrCreateGuildAsync(ctx.Guild.Id, e => e.VoiceMutedUsers);
                config.VoiceMutedUsers.Add(vmu);
                store.Update(config);
                await store.SaveChangesAsync();
            }
            await user.ModifyAsync(user => user.Mute = true);
            TaskQueue.ScheduleTask(vmu, time, VoiceMuteCallback, vmu.Id);

        }

        //public async Task CancelMuteAsync(SocketGuildUser user)
        //{

        //}

        public async Task VoiceMuteCallback(VoiceMutedUser args)
        {

            var guild = DiscordClient.GetGuild(args.GuildID);
            var user = guild.GetUser(args.UserID);

            await user.ModifyAsync(user => user.Mute = false);
            LogService.LogInformation($"UnMuted {user.GetDisplayName()} after {(args.ExpiresAtUTC-args.SetAtUTC).Humanize()}", Guild: guild);

            using (var store = ServiceProvider.GetRequiredService<GuildStore>())
            {
                var config = await store.GetOrCreateGuildAsync(args.GuildID, e => e.VoiceMutedUsers);
                config.VoiceMutedUsers.Remove(args);
                store.Update(config);
                await store.SaveChangesAsync();
            }
        }


    }
}
