﻿using Casino.Common;
using Mummybot.Attributes.Checks;
using Mummybot.Database.Entities;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
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
            [Description("the message you want me to tell you when i remind you")]string message
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
            sb.Append("I will remind you about ").Append(reminder.Message).Append("(id: ").Append(reminder.Id);

            await ReplyAsync(sb.ToString());
        }
    }
}
