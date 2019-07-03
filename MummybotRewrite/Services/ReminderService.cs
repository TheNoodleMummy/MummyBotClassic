﻿using Casino.Common;
using Discord.WebSocket;
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

        public ReminderService(TaskQueue taskQueue, GuildStore guildStore,DiscordSocketClient discordclient,LogService logService)
        {
            TaskQueue = taskQueue;
            GuildStore = guildStore;
            DiscordClient = discordclient;
            LogService = logService;
        }

        public override async Task InitialiseAsync(IServiceProvider services)
        {
            var guildconfigs = await GuildStore.GetAllGuildsAsync(x => x.Reminders);
            foreach (Guild guild in guildconfigs)
            {
                if (guild.UsesReminders)
                {
                    LogService.LogInformation("Executing expired reminders.", LogSource.ReminderService);
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    foreach (Reminder reminder in reminders)
                    {
                        await ReminderCallbackAsync(reminder);
                    }
                }
                else
                {
                    LogService.LogInformation("Executing Expired reminder but guild has turn off reminder => simply removing.", LogSource.ReminderService);
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    foreach (Reminder reminder in reminders)
                    {
                        guild.Reminders.Remove(reminder);
                    }
                }
                GuildStore.Update(guild);
                await GuildStore.SaveChangesAsync();
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
                    sb.Append("minutes, ");
            }
            sb.Append("ago you asked me to remind you about \n").Append(reminder.Message);
            await DiscordClient.GetGuild(reminder.GuildID).GetTextChannel(reminder.ChannelID).SendMessageAsync(sb.ToString());

            var guildconfig = await GuildStore.GetOrCreateGuildAsync(reminder.GuildID,r=>r.Reminders);
            guildconfig.Reminders.Remove(reminder);
            GuildStore.Update(guildconfig);
            await GuildStore.SaveChangesAsync();
        }
    }
}