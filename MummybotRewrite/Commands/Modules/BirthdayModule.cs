using Discord;
using Discord.WebSocket;
using Mummybot.Attributes.Checks;
using Mummybot.Services;
using Qmmands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class BirthdayModule : MummyModule
    {
        [Command("birthdays")]
        [Description("get all birthdays that are registered in this guild")]
        public async Task GetBirthdays()
        {
            var sb = new StringBuilder();
            foreach (var birthday in GuildConfig.Birthdays.OrderBy(x=>x.NextBdayUTC))
            {
                var user = Context.Guild.GetUser(birthday.UserId);
                sb.Append(birthday.BDay.ToString())
                    .Append(" is the birthday of ")
                    .Append(user.GetDisplayName())
                    .Append(".\n");
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("birthday")]
        [Description("check a users birthday")]
        public async Task Birthday(
            [Description("if not passed it will take the authors as user")]SocketGuildUser user = null
            )
        {
            var id = user?.Id ?? Context.User.Id;
            var bday = GuildConfig.Birthdays.Find(b => b.UserId == id);
            if (bday is null && user is null)
                await ReplyAsync("You dont have a birthday set with me here");
            else if (bday is null && user != null)
                await ReplyAsync("this user does not have a birthday registered yet");
            else if (user is null)
                await ReplyAsync($"your birthday is set at {bday.BDay}");
            else
                await ReplyAsync($"{user.GetDisplayName()} birthday is registered at {bday}");
        }

        [Group("register")]
        public class RegisterModule : MummyModule
        {
            public BirthdayService BirthdayService { get; set; }

            [Command("birthday")]
            [Description("register your birthday with mummybot he will wish ou a happy birthday on the exact time your set")]
            public async Task RegisterBirthday(
                [Remainder,Description("the date + time + date format(dmy/mdy) of your bday (ex: 20/10/1994 12:14 dmy")]DateTimeOffset dateTimeOffset)
            {
                var result = await BirthdayService.RegisterBirthdayAsync(Context, dateTimeOffset);
                if (result.IsSuccess)
                    await ReplyAsync($"Added birthday for {Context.User.GetDisplayName()} on {dateTimeOffset.ToString()}");
                else
                    await ReplyAsync(result.ErrorReason);
            }

            [Command("birthday")]
            [RequirePermissions(Enums.PermissionTarget.User,GuildPermission.ManageGuild)]
            public async Task RegisterBirthday(ulong userid, [Remainder]DateTimeOffset dateTimeOffset)
            {
                var result = await BirthdayService.RegisterBirthdayAsync(Context, dateTimeOffset, userid);
                if (result.IsSuccess)
                    await ReplyAsync($"Added birthday for {Context.Guild.GetUser(userid).GetDisplayName()} on {dateTimeOffset.ToString()}");
                else
                    await ReplyAsync(result.ErrorReason);
            }
        }
    }
}
