using Casino.Common;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class ReminderService : BaseService
    {
        public TaskQueue TaskQueue { get; }
        public DiscordSocketClient DiscordClient { get; }

        private readonly LogService LogService;
        private readonly IServiceProvider _services;

        public ReminderService(TaskQueue taskQueue, DiscordSocketClient discordclient, LogService logService, IServiceProvider service)
        {
            TaskQueue = taskQueue;
            DiscordClient = discordclient;
            LogService = logService;
            _services = service;
        }

        public override async Task InitialiseAsync(IServiceProvider services)
        {
            using var GuildStore = _services.GetRequiredService<GuildStore>();

            var guildconfigs = await GuildStore.GetAllGuildsAsync(x => x.Reminders);
            foreach (Guild guild in guildconfigs)
            {

                if (guild.UsesReminders)
                {
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    LogService.LogInformation($"Executing {reminders.Count} expired reminders.", LogSource.ReminderService, guild.GuildID);
                    foreach (Reminder reminder in reminders)
                    {
                        await ReminderCallbackAsync(reminder);
                        guild.Reminders.Remove(reminder);
                    }
                }
                else
                {
                    LogService.LogInformation("Executing Expired reminder but guild has turn off reminder => simply removing.", LogSource.ReminderService, guild.GuildID);
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    LogService.LogInformation($"Removing {reminders.Count} expired reminders", LogSource.ReminderService, guild.GuildID);
                    foreach (Reminder reminder in reminders)
                    {
                        guild.Reminders.Remove(reminder);
                        await GuildStore.SaveChangesAsync();
                    }
                }
                foreach (var item in guild.Reminders)
                {
                    TaskQueue.ScheduleTask(item, item.ExpiresAtUTC, ReminderCallbackAsync);
                }
            }
        }

        public void RegisterReminder(Reminder reminder, ulong id)
        {
            TaskQueue.ScheduleTask(reminder, reminder.ExpiresAtUTC, ReminderCallbackAsync, id);
            LogService.LogInformation($"Set Reminder at {reminder.ExpiresAtUTC}UTC", Enums.LogSource.ReminderService, reminder.GuildID);
        }

        public async Task ReminderCallbackAsync(Reminder reminder)
        {

            LogService.LogInformation("Executing reminder callback", Enums.LogSource.ReminderService, reminder.GuildID);
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
                sb.Append(time.Hours);

                if (time.Hours == 1)
                    sb.Append("Hour, ");
                else
                    sb.Append("Hours, ");
            }

            if (time.Minutes != 0)
            {

                sb.Append(time.Minutes);

                if (time.Minutes == 1)
                    sb.Append("Minute, ");
                else
                    sb.Append("Minutes, ");
            }
            sb.AppendLine("ago you asked me to remind you about:").AppendLine(reminder.Message);
            //if late
            if (reminder.ExpiresAtUTC.Ticks < DateTime.UtcNow.AddMinutes(5).Ticks)
            {
                var late = DateTime.UtcNow - reminder.ExpiresAtUTC;
                sb.Append("but im sorry, im ");
                if (late.Days != 0)
                {
                    hasdays = true;
                    sb.Append(late.Days);
                    if (late.Days == 1)
                        sb.Append("Day");
                    else
                        sb.Append("Days");
                }

                if (late.Hours != 0)
                {
                    sb.Append(", ");
                    hashours = true;
                    sb.Append(late.Hours);

                    if (late.Hours == 1)
                        sb.Append("Hour");
                    else
                        sb.Append("Hours");
                }

                if (late.Minutes != 0)
                {
                    sb.Append(", ");
                    sb.Append(late.Minutes);

                    if (late.Minutes == 1)
                        sb.Append("Minute");
                    else
                        sb.Append("Minutes");
                }
                sb.Append(" late.");
            }
            await DiscordClient.GetGuild(reminder.GuildID).GetTextChannel(reminder.ChannelID).SendMessageAsync(sb.ToString());



            using var GuildStore = _services.GetRequiredService<GuildStore>();
            var guildconfig = await GuildStore.GetOrCreateGuildAsync(reminder.GuildID, r => r.Reminders);
            var remind = guildconfig.Reminders.Find(r => r.Id == reminder.Id);
            guildconfig.Reminders.Remove(remind);
            await GuildStore.SaveChangesAsync();
        }

        public async Task CancelReminder(ulong id, ulong guildid)
        {
            using var guildstore = _services.GetRequiredService<GuildStore>();
            var guildconfig = await guildstore.GetOrCreateGuildAsync(guildid);
            var reminder = guildconfig.Reminders.FirstOrDefault(r => r.Id == id);
            guildconfig.Reminders.Remove(reminder);
            await guildstore.SaveChangesAsync();

            var task = TaskQueue.Queue.FirstOrDefault(task => task.ID == id);
            TaskQueue.Remove(task);

        }
    }
}
