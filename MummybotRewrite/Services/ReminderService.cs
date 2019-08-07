using Casino.Common;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class ReminderService : BaseService
    {
        public TaskQueue TaskQueue { get; }
        public GuildStore GuildStore { get; }
        public DiscordSocketClient DiscordClient { get; }
        private readonly IServiceProvider _services;

        public ReminderService(TaskQueue taskQueue, GuildStore guildStore,DiscordSocketClient discordclient,LogService logService,IServiceProvider service)
        {
            TaskQueue = taskQueue;
            GuildStore = guildStore;
            DiscordClient = discordclient;
            LogService = logService;
            _services = service;
        }

        public override async Task InitialiseAsync(IServiceProvider services)
        {
            var guildconfigs = await GuildStore.GetAllGuildsAsync(x => x.Reminders);
            foreach (Guild guild in guildconfigs)
            {
                if (guild.UsesReminders)
                {
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    LogService.LogInformation($"Executing {reminders.Count} expired reminders.", LogSource.ReminderService);
                    foreach (Reminder reminder in reminders)
                    {
                        await ReminderCallbackAsync(reminder);
                        guild.Reminders.Remove(reminder);
                    }
                }
                else
                {
                    LogService.LogInformation("Executing Expired reminder but guild has turn off reminder => simply removing.", LogSource.ReminderService);
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    LogService.LogInformation($"Removing {reminders.Count} expired reminders", LogSource.ReminderService);
                    foreach (Reminder reminder in reminders)
                    {
                        guild.Reminders.Remove(reminder);
                    }
                }
                GuildStore.Update(guild);
                await GuildStore.SaveChangesAsync();

                foreach (var item in guild.Reminders)
                {
                    TaskQueue.ScheduleTask(item, item.ExpiresAtUTC, ReminderCallbackAsync);
                }
            }
        }

        public void RegisterReminder(Reminder reminder,ulong id)
        {
            TaskQueue.ScheduleTask(reminder, reminder.ExpiresAtUTC, ReminderCallbackAsync,id);
            LogService.LogInformation($"Set Reminder at {reminder.ExpiresAtUTC}UTC", Enums.LogSource.ReminderService);
        }

        public async Task ReminderCallbackAsync(Reminder reminder)
        {
            LogService.LogInformation("Executing reminder callback", Enums.LogSource.ReminderService);
            StringBuilder sb = new StringBuilder();
            bool hasdays = false, hashours = false;
            var time = DateTime.UtcNow - reminder.SetAtUTC;
            sb.Append("yoo ").Append(DiscordClient.GetUser(reminder.UserID).Mention).Append(", ");
            if (time.Days != 0)
            {
                hasdays = true;
                sb.Append(time.Days);
                if (time.Days == 1)
                    sb.Append("Day, ");
                else
                    sb.Append("Days, ");
            }

            if (time.Hours != 0)
            {
                hashours = true;
                if (hasdays)
                    sb.Append(time.Hours);
                else
                    sb.Append(time.Hours);

                if (time.Hours == 1)
                    sb.Append("Hour, ");
                else
                    sb.Append("Hours, ");
            }

            if (time.Minutes != 0)
            {
                if (hashours)
                    sb.Append(time.Minutes);
                else
                    sb.Append(time.Minutes);

                if (time.Minutes == 1)
                    sb.Append("Minute, ");
                else
                    sb.Append("Minutes, ");
            }
            sb.Append("ago you asked me to remind you about: \n").Append(reminder.Message);
            await DiscordClient.GetGuild(reminder.GuildID).GetTextChannel(reminder.ChannelID).SendMessageAsync(sb.ToString());

            var store = _services.GetRequiredService<GuildStore>();
            var guildconfig = await store.GetOrCreateGuildAsync(reminder.GuildID,r=>r.Reminders);
            guildconfig.Reminders.Remove(reminder);
            store.Update(guildconfig);
            await store.SaveChangesAsync();
            store.Dispose();
        }
    }
}
