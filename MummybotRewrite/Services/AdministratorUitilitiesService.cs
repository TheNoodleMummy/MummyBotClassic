using Casino.Common;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    [InitilizerPriority(2)]
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
                    var vmus = guild.VoiceMutedUsers.Where(r => r.ExpiresAtUTC < DateTimeOffset.UtcNow).ToList();
                    LogService.LogInformation($"unmuting {vmus.Count} expired VoiceMutes.", LogSource.AdministratotUtilService, guild.GuildID);
                    foreach (var vmu in vmus)
                    {
                        await VoiceMuteCallback(vmu);
                        guild.VoiceMutedUsers.Remove(vmu);
                    }
                    var vdus = guild.VoiceDeafenedUsers.Where(r => r.ExpiresAtUTC < DateTimeOffset.UtcNow).ToList();
                    LogService.LogInformation($"undeafening {vmus.Count} expired VoiceDeafens.", LogSource.AdministratotUtilService, guild.GuildID);
                    foreach (var vdu in vdus)
                    {
                        await VoiceDeafCallback(vdu);
                        guild.VoiceDeafenedUsers.Remove(vdu);
                    }


                    foreach (var item in guild.VoiceDeafenedUsers)
                    {
                        TaskQueue.ScheduleTask(item, item.ExpiresAtUTC, VoiceDeafCallback,item.Id);
                    }
                    foreach (var item in guild.VoiceMutedUsers)
                    {
                        TaskQueue.ScheduleTask(item, item.ExpiresAtUTC, VoiceMuteCallback,item.Id);
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
            LogService.LogInformation($"Muted {user.GetDisplayName()} for {time.Humanize()}", LogSource.AdministratotUtilService, ctx.GuildId);

            using (var store = ServiceProvider.GetRequiredService<GuildStore>())
            {
                var config = await store.GetOrCreateGuildAsync(ctx.Guild.Id, e => e.VoiceMutedUsers);
                config.VoiceMutedUsers.Add(vmu);
                await store.SaveChangesAsync();
            }
            await user.ModifyAsync(user => user.Mute = true);
            TaskQueue.ScheduleTask(vmu, time, VoiceMuteCallback, vmu.Id);

        }

        public async Task VoiceDeafen(MummyContext ctx, SocketGuildUser user, TimeSpan time)
        {
            var vdu = new VoiceDeafUser()
            {
                Id = SnowFlakeGenerator.NextLong(),
                ChannelID = ctx.Channel.Id,
                SetAtUTC = DateTimeOffset.UtcNow,
                ExpiresAtUTC = DateTimeOffset.UtcNow.Add(time),
                UserID = user.Id,
                GuildID = ctx.Guild.Id
            };

            await ctx.Channel.SendMessageAsync($"Deafened {user.GetDisplayName()} for {time.Humanize()}");
            LogService.LogInformation($"Deafened {user.GetDisplayName()} for {time.Humanize()}", LogSource.AdministratotUtilService, ctx.GuildId);

            using (var store = ServiceProvider.GetRequiredService<GuildStore>())
            {
                var config = await store.GetOrCreateGuildAsync(ctx.Guild.Id, e => e.VoiceMutedUsers);
                config.VoiceDeafenedUsers.Add(vdu);
                await store.SaveChangesAsync();
            }
            await user.ModifyAsync(user => user.Deaf = true);
            TaskQueue.ScheduleTask(vdu, time, VoiceDeafCallback, vdu.Id);

        }       

        public async Task VoiceMuteCallback(VoiceMutedUser args)
        {

            var guild = DiscordClient.GetGuild(args.GuildID);
            var user = guild.GetUser(args.UserID);

            await user.ModifyAsync(user => user.Mute = false);
            LogService.LogInformation($"UnMuted {user.GetDisplayName()} after {(args.ExpiresAtUTC - args.SetAtUTC).Humanize()}",LogSource.AdministratotUtilService, guild.Id);

            using var store = ServiceProvider.GetRequiredService<GuildStore>();
            var config = await store.GetOrCreateGuildAsync(args.GuildID, e => e.VoiceMutedUsers);
            config.VoiceMutedUsers.Remove(args);
            await store.SaveChangesAsync();
        }

        public async Task VoiceDeafCallback(VoiceDeafUser args)
        {

            var guild = DiscordClient.GetGuild(args.GuildID);
            var user = guild.GetUser(args.UserID);

            await user.ModifyAsync(user => user.Deaf = false);
            LogService.LogInformation($"Undeafened {user.GetDisplayName()} after {(args.ExpiresAtUTC - args.SetAtUTC).Humanize()}",LogSource.AdministratotUtilService, guild.Id);

            using var store = ServiceProvider.GetRequiredService<GuildStore>();
            var config = await store.GetOrCreateGuildAsync(args.GuildID, e => e.VoiceMutedUsers);
            config.VoiceDeafenedUsers.Remove(args);
            await store.SaveChangesAsync();
        }

    }
}
