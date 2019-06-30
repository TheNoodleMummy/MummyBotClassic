using Casino.Common;
using Discord.WebSocket;
using Mummybot.Database;
using Mummybot.Database.Entities;
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
        public LogService LogService { get; }

        public ReminderService(TaskQueue taskQueue, GuildStore guildStore,DiscordSocketClient discordclient,LogService logService)
        {
            TaskQueue = taskQueue;
            GuildStore = guildStore;
            DiscordClient = discordclient;
            LogService = logService;
        }

        public override async Task InitialiseAsync(IServiceProvider services)
        {
            var guildconfigs = await GuildStore.GetAlllGuildsAsync(x => x.Reminders);
            foreach (Guild guild in guildconfigs)
            {
                if (guild.UsesReminders)
                {
                    var reminders = guild.Reminders.Where(r => r.ExpiresAtUTC < DateTime.UtcNow).ToList();
                    foreach (Reminder reminder in reminders)
                    {
                        await ReminderCallbackAsync(reminder);
                    }
                }
                else
                {
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
        }

        public async Task ReminderCallbackAsync(Reminder reminder)
        {
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
            sb.Append("ago you asked me to remind you about ").Append(reminder.Message);
            await DiscordClient.GetGuild(reminder.GuildID).GetTextChannel(reminder.ChannelID).SendMessageAsync(sb.ToString());

            var guildconfig = await GuildStore.GetOrCreateGuildAsync(reminder.GuildID,r=>r.Reminders);
            guildconfig.Reminders.Remove(reminder);
            GuildStore.Update(guildconfig);
            await GuildStore.SaveChangesAsync();
        }
    }
}
