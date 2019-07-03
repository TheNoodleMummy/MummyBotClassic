using Casino.Common;
using Discord;
using Discord.WebSocket;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Database.Entities;
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
        private readonly GuildStore _guildStore;
        private readonly DiscordSocketClient _discordClient;
        private readonly SnowFlakeGeneratorService _snowFlakeGenerator;

        public BirthdayService(TaskQueue taskQueue, GuildStore guildStore,DiscordSocketClient discord,LogService logService,SnowFlakeGeneratorService snowFlake)
        {
            _taskQueue = taskQueue;
            _guildStore = guildStore;
            _discordClient = discord;
            LogService = logService;
            _snowFlakeGenerator = snowFlake;
        }

        public override Task InitialiseAsync(IServiceProvider services)
            => NextBirthday();

        /// <summary>
        /// tries to register a new birthday
        /// </summary>
        internal async Task<BirthdayResult> RegisterBirthdayAsync(MummyContext context, DateTimeOffset dateTimeOffset, ulong userid = 0)
        {
            var guildconfig = await _guildStore.GetOrCreateGuildAsync(context.Guild.Id,x=>x.Birthdays);
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
                _guildStore.Update(guildconfig);
                await _guildStore.SaveChangesAsync();
                var dbbday = guildconfig.Birthdays.Find(b => b.Id == id);
                return new BirthdayResult() { Birthday = dbbday, IsSuccess = true };
            }
            catch (Exception)
            {
                return new BirthdayResult() { Birthday = null, IsSuccess = false, Error= "Something went wrong"};
            }
        }

        /// <summary>
        /// make the bday service register the next bday in line
        /// </summary>
        private async Task NextBirthday()
        {
            var bdays = (await _guildStore.GetAllGuildsAsync(x => x.Birthdays)).SelectMany(x => x.Birthdays).OrderByDescending(b => b.NextBdayUTC.ToUniversalTime());
            var expiredbdays = bdays.Where(b => b.NextBdayUTC < DateTimeOffset.UtcNow);
            foreach (var birthday in expiredbdays)
            {
                await BirthdayCallbackAsync(birthday);
            }

            var bday = (await _guildStore.GetAllGuildsAsync(x => x.Birthdays))
                .SelectMany(x => x.Birthdays)
                .OrderByDescending(b => b.NextBdayUTC.ToUniversalTime())
                .FirstOrDefault(b=>b.NextBdayUTC > DateTimeOffset.UtcNow);

            if (bday is null)
            {
                LogService.LogInformation("no birthday found waiting for first add", Enums.LogSource.BirthdayService);
            }
            else
                _taskQueue.ScheduleTask(bday, bday.NextBdayUTC, BirthdayCallbackAsync);
        }

        /// <summary>
        /// callback for the TaskQueue, thats executed when its there birthday
        /// </summary>
        /// <param name="birthday"></param>
        public async Task BirthdayCallbackAsync(Birthday birthday)
        {
            var guildconfig = await _guildStore.GetOrCreateGuildAsync(birthday.GuildID, x => x.Birthdays);
            if (!guildconfig.UsesBirthdays)
                return;
            var age = DateTimeOffset.UtcNow.Year - birthday.BDay.ToUniversalTime().Year;
            var guild = _discordClient.GetGuild(birthday.GuildID);
            if (guildconfig.BdayChannelId == 0)
            {
                var channels = guild.TextChannels.OrderBy(c => c.Position);
                var channel = channels.FirstOrDefault(c=>c.PermissionOverwrites.Any(p=>p.Permissions.SendMessages == PermValue.Allow));
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
            _guildStore.Update(guildconfig);
            await _guildStore.SaveChangesAsync();
        }
    }

    public class BirthdayResult
    {
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
        public Birthday Birthday { get; set; }
    }
}
