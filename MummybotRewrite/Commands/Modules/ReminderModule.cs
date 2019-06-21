using Discord;
using Mummybot.Services;
using Qmmands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class ReminderModule : MummyBase
    {
        public ReminderService ReminderS { get; set; }

        [Command("remind")]
        [RunMode(RunMode.Parallel)]
        public async Task Remind(TimeSpan time, [Remainder]string message)
        {


            bool hasdays = false;
            bool hashours = false;
            var remind = await ReminderS.NewReminderAsync(userid: Context.User.Id, message: message, channelid: Context.Channel.Id, guildid: Context.Guild.Id, expiresat: DateTime.UtcNow.Add(time), jumpurl: Context.Message.GetJumpUrl());
            Logs.LogInformation($"current time {DateTime.Now} reminder set in {time} at {remind.When}", Enums.LogSource.ReminderService);

            StringBuilder sb = new StringBuilder();

            sb.Append($"Alright {Context.User.Mention}, ");
            if (time.Days != 0)
            {
                hasdays = true;
                sb.Append($"in {time.Days}");
                if (time.Days == 1)
                    sb.Append("Day, ");
                else
                    sb.Append("Days, ");
            }

            if (time.Hours != 0)
            {
                hashours = true;
                if (hasdays)
                    sb.Append("and " + time.Hours);
                else
                    sb.Append($"in {time.Hours}");

                if (time.Hours == 1)
                    sb.Append("Hour, ");
                else
                    sb.Append("Hours, ");
            }

            if (time.Minutes != 0)
            {
                if (hashours)
                    sb.Append("and " + time.Minutes);
                else
                    sb.Append($"in {time.Minutes}");


                if (time.Minutes == 1)
                    sb.Append("Minute, ");
                else
                    sb.Append("minutes, ");
            }
            sb.Append($"I will remind you about {remind.Message}");

            await ReplyAsync(sb.ToString());
        }

        [Command("reminders")]
        [RunMode(RunMode.Parallel)]
        public async Task GetReminders()
        {
            var reminders = GuildConfig.Reminders.Where(r => r.UserID == Context.User.Id).OrderBy(r => r.When);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Context.User}'s current set reminder('s),");
            if (reminders.Count() == 0)
            {
                sb.AppendLine("you currently have no reminders set");
            }
            else
            {
                sb.AppendLine($"you currently have {reminders.Count()}");
            }
            foreach (var reminder in reminders)
            {
                sb.AppendLine($"{reminder.Message} - {reminder.When} UTC");
            }
            await ReplyAsync(sb.ToString());
        }
    }
}
