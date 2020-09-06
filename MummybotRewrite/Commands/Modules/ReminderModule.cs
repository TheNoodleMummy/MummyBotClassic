using Casino.Common;
using Discord;
using Discord.Addons.Interactive;
using Mummybot.Attributes.Checks;
using Mummybot.Database.Entities;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireReminders]
    public class ReminderModule : MummyModule
    {
        public ReminderService ReminderService { get; set; }
        public SnowFlakeGeneratorService SnowFlakeGenerator { get; set; }

        [Command("remind")]
        [Description("set a reminder in the future the bot will then remind you when its time")]
        public async Task RemindAsync(
            [Description("how long it should be before your reminded (ex: 5s, 5min/m, 5hour/h")]TimeSpan time,
            [Description("the message you want me to tell you when i remind you"),Remainder]string message
            )
        {
            var reminder = new Reminder
            {
                ChannelID = Context.Channel.Id,
                GuildID = Context.Guild.Id,
                Message = message,
                SetAtUTC = DateTime.UtcNow,
                ExpiresAtUTC = DateTime.UtcNow + time,
                Id = SnowFlakeGenerator.NextLong(),
                UserID = Context.User.Id
            };
            GuildConfig.Reminders.Add(reminder);
            ReminderService.RegisterReminder(reminder, reminder.Id);

            StringBuilder sb = new StringBuilder();
            bool hasdays=false, hashours = false;

            sb.Append("Alright ").Append(Context.User.Mention).Append(", ");
            if (time.Days != 0)
            {
                hasdays = true;
                sb.Append("in ").Append(time.Days);
                if (time.Days == 1)
                    sb.Append("Day, ");
                else
                    sb.Append("Days, ");
            }

            if (time.Hours != 0)
            {
                hashours = true;
                if (hasdays)
                    sb.Append("and ").Append(time.Hours);
                else
                    sb.Append("in ").Append(time.Hours);

                if (time.Hours == 1)
                    sb.Append("Hour, ");
                else
                    sb.Append("Hours, ");
            }

            if (time.Minutes != 0)
            {
                if (hashours)
                    sb.Append("and ").Append(time.Minutes);
                else
                    sb.Append("in ").Append(time.Minutes);

                if (time.Minutes == 1)
                    sb.Append("Minute, ");
                else
                    sb.Append("minutes, ");
            }
            sb.Append("I will remind you about ").Append(reminder.Message);

            await ReplyAsync(sb.ToString());
        }

        [Command("remind"),Priority(100000)]
        [Description("set a reminder in the future the bot will then remind you when its time")]
        public async Task RemindAsync(
           [Description("when you want me to remind you \n(ex: 24/2/2007 7:23:57 PM +00:00 dmy,\n  2/24/2007 7:23:57 PM +00:00 mdy )")]DateTimeOffset time,
           [Description("the message you want me to tell you when i remind you"), Remainder]string message
           )
        {
            var reminder = new Reminder
            {
                ChannelID = Context.Channel.Id,
                GuildID = Context.Guild.Id,
                Message = message,
                SetAtUTC = DateTime.UtcNow,
                ExpiresAtUTC =time,
                Id = SnowFlakeGenerator.NextLong(),
                UserID = Context.User.Id,
                JumpUrl = Context.Message.GetJumpUrl()
            };

            GuildConfig.Reminders.Add(reminder);
            ReminderService.RegisterReminder(reminder, reminder.Id);

            StringBuilder sb = new StringBuilder();
            bool hasdays = false, hashours = false;
            var timespan = time.ToUniversalTime() - DateTimeOffset.UtcNow;
            sb.Append("Alright ").Append(Context.User.Mention).Append(", ");
            if (timespan.Days != 0)
            {
                hasdays = true;
                sb.Append("in ").Append(timespan.Days);
                if (timespan.Days == 1)
                    sb.Append("Day, ");
                else
                    sb.Append("Days, ");
            }

            if (timespan.Hours != 0)
            {
                hashours = true;
                if (hasdays)
                    sb.Append("and ").Append(timespan.Hours);
                else
                    sb.Append("in ").Append(timespan.Hours);

                if (timespan.Hours == 1)
                    sb.Append("Hour, ");
                else
                    sb.Append("Hours, ");
            }

            if (timespan.Minutes != 0)
            {
                if (hashours)
                    sb.Append("and ").Append(timespan.Minutes);
                else
                    sb.Append("in ").Append(timespan.Minutes);

                if (timespan.Minutes == 1)
                    sb.Append("Minute, ");
                else
                    sb.Append("minutes, ");
            }
            sb.Append("I will remind you about ").Append(reminder.Message).Append("(id: ").Append(reminder.Id).Append(")");

            await ReplyAsync(sb.ToString());
        }

        [Command("reminders")]
        [Description("see all your current reminders")]
        public async Task RemindersAsync()
        {
            var reminders = new SortedList<int, Reminder>();
            for (int i = 0; i < GuildConfig.Reminders.Where(r => r.UserID == Context.UserId).Count(); i++)
            {
                reminders.Add(i, GuildConfig.Reminders[i]);
            }

            if (reminders.Count() == 0)
                await ReplyAsync("You currently have no reminders set");

            if (reminders.Count() <= 10)
            {

                string list = "";
                foreach (var reminder in reminders.Values)
                {
                    string message = "";
                    if (reminder.Message.Length <= 100)                    
                        message = reminder.Message;
                    else
                        message = reminder.Message.Substring(0, 100) + "...";
                    
                    list += $"{reminder.Id.ToString().PadLeft(15)} | {reminder.ExpiresAtUTC}UTC {"".PadRight(10)} | {message}\n";
                }
                var eb = new EmbedBuilder().WithAuthor(Context.User).WithDescription($"```{list}```");
                await ReplyAsync(embed: eb);
            }
            //else
            //{
            //    for (int i = 0; i < reminders.Count; i++)
            //    {
            //        string list = "";
            //        foreach (var reminder in reminders.Values)
            //        {
            //            list += $"{reminder.Id.ToString().PadLeft(5)} | {reminder.ExpiresAtUTC}UTC {"".PadRight(10)} | {reminder.Message.Take(100)}\n";
            //        }
            //    }

            //}
            
        }
    }
}
