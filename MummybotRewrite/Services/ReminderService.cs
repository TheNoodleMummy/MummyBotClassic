using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Mummybot.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Database;
using Mummybot.interfaces;
using System.Threading.Tasks;
using Mummybot.Attributes;
using System.Collections.Generic;

namespace Mummybot.Services
{
    [Service("Reminder Service",typeof(ReminderService))]
    public class ReminderService : IRemoveableService
    {
        [Inject]
       public DiscordSocketClient _Client { get; set; }

        [Inject]
        public TimerService TimerService { get; set; }

        [Inject]
        public DBService Database { get; set; }

        [Inject]
        public MessagesService MessagesService { get; set; }


        public async Task InitializeAsync()
        {
            var toRemove = new List<Reminder>();

            foreach (var guild in _Client.Guilds)
            {
                var reminders = Database.GetGuildUncached(guild.Id).Reminders;
                foreach (var reminder in reminders)
                {
                    var initializedReminder = new Reminder(reminder, this);
                    if (reminder.When < DateTime.Now)
                    {
                        toRemove.Add(initializedReminder);
                        continue;
                    }

                    await TimerService.EnqueueAsync(initializedReminder);
                }
            }

            foreach (var reminder in toRemove)
                await reminder.RemoveAsync();

        }

       

        public async Task<Reminder> NewReminderAsync(string message,ulong userid,ulong channelid,ulong guildid,DateTime expiresat,string jumpurl)        {
            var reminder = new Reminder(this)
            {
                Guildid = guildid,
                ChannelID = channelid,
                UserID = userid,
                Message = message,
                When = expiresat,
                SetOnUTC = DateTime.UtcNow,
                JumpUrl = jumpurl
            };

           await TimerService.EnqueueAsync(reminder);

            var guild = Database.GetGuildUncached(guildid);
            guild.Reminders.Add(reminder);
            Database.UpsertGuild(guild);
            return reminder;
        }

        public async Task RemoveAsync(IRemoveable obj)
        {
            if (!(obj is Reminder reminder)) return;
            var guild = _Client.GetGuild(reminder.Guildid);
                var user = guild.GetUser(reminder.UserID);
            var time = DateTime.UtcNow - reminder.SetOnUTC;
                StringBuilder sb = new StringBuilder();

            sb.Append(user.Mention + ", ");
            var weeks = Math.Floor((double)time.Days / 7);
            var daysleft = time.Days % 7;
            if (weeks != 0)
            {
                if (weeks ==1)
                    sb.Append("1 week,");        
                else
                    sb.Append($"{weeks} weeks,");
            }
            if (daysleft != 0)
            {
                if (daysleft == 1)
                    sb.Append("1 day,");
                else
                    sb.Append($"{daysleft} day,");
            }
            if (time.Hours != 0)
            {
                if (time.Hours == 1)
                    sb.Append("1 hour,");
                else
                    sb.Append($"{time.Hours} hours,");
            }
            if (time.Minutes != 0)
            {
                if (time.Minutes == 1)
                    sb.Append("1 minute,");
                else
                    sb.Append($"{time.Minutes} minutes,");
            }
            if (time.Seconds != 0)
            {
                if (time.Seconds == 1)
                    sb.Append("1 second,");
                else
                    sb.Append($"{time.Seconds} seconds,");
            }
           


            sb.Append($"ago you asked to be reminded about {reminder.Message}.");

            await MessagesService.NewMessageAsync(reminder.UserID, 0, reminder.ChannelID, sb.ToString());
            var guildsettings = Database.GetGuildUncached(guild.Id);
            guildsettings.Reminders.Remove(reminder);
            Database.UpsertGuild(guildsettings);
        }

       
    }

}