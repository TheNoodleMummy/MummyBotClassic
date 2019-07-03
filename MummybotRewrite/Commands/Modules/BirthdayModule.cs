using Discord.WebSocket;
using Mummybot.Services;
using Qmmands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class BirthdayModule : MummyModule
    {
        [Command("birthdays")]
        public async Task GetBirthdays()
        {
            var sb = new StringBuilder();
            foreach (var birthday in GuildConfig.Birthdays)
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
        public async Task Birthday(SocketGuildUser user = null)
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
            public async Task RegisterBirthday([Remainder]DateTimeOffset dateTimeOffset)
            {
                var result = await BirthdayService.RegisterBirthdayAsync(Context, dateTimeOffset);
                if (result.IsSuccess)
                    await ReplyAsync($"Added birthday for {Context.User.GetDisplayName()} on {dateTimeOffset.ToString()}");
                else
                    await ReplyAsync(result.Error);
            }
        }
    }
}
