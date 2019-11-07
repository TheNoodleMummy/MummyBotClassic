using Casino.Common;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class BirthdayService : BaseService
    {
        private readonly TaskQueue _taskQueue;
        private readonly DiscordSocketClient _discordClient;
        private readonly LogService LogService;
        private readonly SnowFlakeGeneratorService _snowFlakeGenerator;
        private readonly IServiceProvider serviceProvider;

        public BirthdayService(
            TaskQueue taskQueue,
            DiscordSocketClient discord,
            LogService logService,
            SnowFlakeGeneratorService snowFlake,
            IServiceProvider services)
        {
            _taskQueue = taskQueue;
            _discordClient = discord;
            LogService = logService;
            _snowFlakeGenerator = snowFlake;
            serviceProvider = services;
        }

        public override Task InitialiseAsync(IServiceProvider services)
            => NextBirthday();

        /// <summary>
        /// tries to register a new birthday
        /// </summary>
        internal async Task<BirthdayResult> RegisterBirthdayAsync(MummyContext context, DateTimeOffset dateTimeOffset, ulong userid = 0)
        {
            using var guildstore = serviceProvider.GetRequiredService<GuildStore>();

            var guildconfig = await guildstore.GetOrCreateGuildAsync(context.Guild.Id, x => x.Birthdays);
            Birthday birthday;
            ulong id = _snowFlakeGenerator.NextLong();
            DateTimeOffset nextbdayUTC = dateTimeOffset.AddYears(DateTimeOffset.UtcNow.Year - dateTimeOffset.ToUniversalTime().Year);
            if (userid == 0)
                birthday = new Birthday() { BDay = dateTimeOffset, UserId = context.User.Id, Id = id, NextBdayUTC = nextbdayUTC };
            else
                birthday = new Birthday() { BDay = dateTimeOffset, UserId = userid, Id = id, NextBdayUTC = nextbdayUTC };
            try
            {
                guildconfig.Birthdays.Add(birthday);
                guildstore.Update(guildconfig);
                var dbbday = guildconfig.Birthdays.Find(b => b.Id == id);
                return new BirthdayResult() { Birthday = dbbday, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new BirthdayResult() { Birthday = null, IsSuccess = false, ErrorReason = "Something went wrong", Exception = ex };
            }
        }

        /// <summary>
        /// make the bday service register the next bday in line
        /// </summary>
        private async Task NextBirthday()
        {
            using var guildstore = serviceProvider.GetRequiredService<GuildStore>();

            var bdays = (await guildstore.GetAllGuildsAsync(x => x.Birthdays)).SelectMany(x => x.Birthdays).OrderByDescending(b => b.NextBdayUTC.ToUniversalTime());
            var expiredbdays = bdays.Where(b => b.NextBdayUTC < DateTimeOffset.UtcNow);
            foreach (var birthday in expiredbdays)
            {
                await ExpiredBirthdayCallbackAsync(birthday);
            }

            var bday = (await guildstore.GetAllGuildsAsync(x => x.Birthdays))
                .SelectMany(x => x.Birthdays)
                .OrderBy(b => b.NextBdayUTC.ToUniversalTime())
                .FirstOrDefault(b => b.NextBdayUTC > DateTimeOffset.UtcNow);

            if (bday is null)
            {
                LogService.LogInformation("no birthday found waiting for first add", Enums.LogSource.BirthdayService);
            }
            else
            {
                LogService.LogInformation($"registered next birthday for {bday.NextBdayUTC} userid: {bday.UserId}",Enums.LogSource.BirthdayService, bday.GuildID);
                _taskQueue.ScheduleTask(bday, bday.NextBdayUTC, BirthdayCallbackAsync);
            }
           
        }

        /// <summary>
        /// callback for when a bday expired :( (bot was offline at the moment of the bday itself)
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        private async Task ExpiredBirthdayCallbackAsync(Birthday birthday)
        {
            using var guildstore = serviceProvider.GetRequiredService<GuildStore>();

            var guildconfig = await guildstore.GetOrCreateGuildAsync(birthday.GuildID, x => x.Birthdays);
            if (!guildconfig.UsesBirthdays)
                return;
            var age = DateTimeOffset.UtcNow.Year - birthday.BDay.ToUniversalTime().Year;
            var guild = _discordClient.GetGuild(birthday.GuildID);
            if (guildconfig.BdayChannelId == 0)
            {
                LogService.LogInformation("no bday channel configured", Enums.LogSource.BirthdayService, guild.Id);
                var channels = guild.TextChannels.OrderBy(c => c.Position);
                var channel = channels.FirstOrDefault(c => c.PermissionOverwrites.Any(p => p.Permissions.SendMessages == PermValue.Allow));
                await channel.SendMessageAsync($"sorry im late by {(DateTimeOffset.UtcNow - birthday.NextBdayUTC).TotalSeconds}seconds please forgive me.{Environment.NewLine}" +
                    $"Anyway Happy {age} Birthday {guild.GetUser(birthday.UserId).Mention}");
            }
            else
            {
                var channel = guild.GetTextChannel(guildconfig.BdayChannelId);
                await channel.SendMessageAsync($"sorry im late by {(DateTimeOffset.UtcNow - birthday.NextBdayUTC).TotalSeconds}seconds please forgive me.{Environment.NewLine}" +
                    $"Anyway Happy {age} Birthday {guild.GetUser(birthday.UserId).Mention}");
            }

            var dbbirthday = guildconfig.Birthdays.FirstOrDefault(b => b.Id == birthday.Id);
            dbbirthday.NextBdayUTC = dbbirthday.NextBdayUTC.AddYears(1);
            guildstore.Update(guildconfig);
        }

        /// <summary>
        /// callback for the TaskQueue, thats executed when its there birthday
        /// </summary>
        /// <param name="birthday"></param>
        public async Task BirthdayCallbackAsync(Birthday birthday)
        {
            using var guildstore = serviceProvider.GetRequiredService<GuildStore>();

            var guildconfig = await guildstore.GetOrCreateGuildAsync(birthday.GuildID, x => x.Birthdays);
            if (!guildconfig.UsesBirthdays)
                return;
            var age = DateTimeOffset.UtcNow.Year - birthday.BDay.ToUniversalTime().Year;
            var guild = _discordClient.GetGuild(birthday.GuildID);
            if (guildconfig.BdayChannelId == 0)
            {
                var channels = guild.TextChannels.OrderBy(c => c.Position);
                var channel = channels.FirstOrDefault(c => c.PermissionOverwrites.Any(p => p.Permissions.SendMessages == PermValue.Allow));
                await channel.SendMessageAsync($"Happy {age} Birthday {guild.GetUser(birthday.UserId).Mention}");
            }
            else
            {
                var channel = guild.GetTextChannel(guildconfig.BdayChannelId);
                await channel.SendMessageAsync($"Happy {age} Birthday {guild.GetUser(birthday.UserId).Mention}");
            }

            birthday.NextBdayUTC = birthday.NextBdayUTC.AddYears(1);
            var bday = guildconfig.Birthdays.Find(b => b.Id == birthday.Id);
            bday = birthday;
            guildstore.Update(guildconfig);
            await NextBirthday();
        }
    }

    public class BirthdayResult : IMummyResult
    {
        public bool IsSuccess { get; set; }
        public Birthday Birthday { get; set; }
        public string ErrorReason { get; set; }
        public Exception Exception { get; set; }
    }
}
