using Discord;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Mummybot.Attributes;
using Mummybot.Database.Models;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Birthday module"), Description("has some commands that have to do with birthdays from people in this guild")]
    public class BdayModule : MummyBase
    {

        public InteractiveService InteractiveService { get; set; }

        [Command("removebday")]
        public async Task RemoveBdayAsync(IGuildUser user = null)
        {
            user = user ?? Context.User;
            if (user == Context.User || (Context.User as SocketGuildUser).GuildPermissions.ManageGuild)
            {
                var bday = GuildConfig.Birthdays.FirstOrDefault(x => x.UserID == user.Id);
                if (bday != null)
                {
                    GuildConfig.Birthdays.Remove(bday);
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
            }
        }
        [Command("bdaysservice")]
        [RunMode(RunMode.Parallel)]
        public async Task EnableStarboard()
        {
            if (GuildConfig.UsesBirthdays)
            {
                var msg = await ReplyAsync("Are you sure you want to disable the BirthdayService?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UsesBirthdays = false;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
            else
            {
                var msg = await ReplyAsync("Are you sure you want to enable the BirthdayService?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UsesBirthdays = true;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await Messages.DeleteMessageAsync(Context, response);
            }
        }

        [Command("bdays"), RequireActiveBdayService]
        public async Task Bdays()
        {
            var bdays = GuildConfig.Birthdays.ToList();
            StringBuilder sb = new StringBuilder();
            var dict = new Dictionary<int, Birthday>();
            if (bdays.Count == 0)
            {
                await ReplyAsync("currently no birthdays registered yet");
                return;
            }
            foreach (var Bday in bdays)
            {


                int daystillbday = 0;
                if (Bday.Bday.DayOfYear < DateTime.Now.DayOfYear)
                {
                    var lastday = new DateTime(DateTime.Now.Year, 12, 31);
                    var firstdayofnextyear = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    var daysleftdaysthisyear = lastday.DayOfYear - DateTime.Now.DayOfYear;
                    var daysleftnextyear = 0;

                    if (DateTime.IsLeapYear(Bday.Bday.Year))
                        daysleftnextyear = (firstdayofnextyear.DayOfYear) + (Bday.Bday.DayOfYear - 1);
                    else
                        daysleftnextyear = firstdayofnextyear.DayOfYear + Bday.Bday.DayOfYear;


                    daystillbday = daysleftdaysthisyear + daysleftnextyear;
                    //sb.AppendLine($"{Context.Client.GetUser(Bday.UserID).Username}'s in {daystillbday} days on {Bday.Bday.ToString("dd/MM/yyyy")} (dmy).");
                    dict.Add(daystillbday, Bday);

                }
                else if (Bday.Bday.DayOfYear == DateTime.Now.DayOfYear)
                {
                    dict.Add(0, Bday);
                    //sb.AppendLine($"{Context.Client.GetUser(Bday.UserID).Username}'s Bday is today wish them a happy birthday boi.");
                }
                else if (Bday.Bday.DayOfYear > DateTime.Now.DayOfYear)
                {
                    if (DateTime.IsLeapYear(Bday.Bday.Year))
                        daystillbday = ((Bday.Bday.DayOfYear - 1) - DateTime.Now.DayOfYear);
                    else
                        daystillbday = (Bday.Bday.DayOfYear - DateTime.Now.DayOfYear);

                    dict.Add(daystillbday, Bday);
                    //sb.AppendLine($"{Context.Client.GetUser(Bday.UserID).Username}'s in {daystillbday} days on {Bday.Bday.ToString("dd/MM/yyyy")} (dmy).");
                }
                dict = dict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            }
            foreach (var item in dict)
            {
                if (item.Key == 0)
                {
                    sb.AppendLine($"{(Context.Client.GetUser(item.Value.UserID)).Username}'s Birthday is today wish them a Happy Birthday boi.");
                }
                if (item.Key == 1)
                {
                    sb.AppendLine($"{(Context.Client.GetUser(item.Value.UserID)).Username}'s Birthday is tomorrow. ({item.Value.Bday.Date.ToShortDateString()})");

                }
                if (item.Key > 1)
                {
                    sb.AppendLine($"{(Context.Client.GetUser(item.Value.UserID)).Username}'s Birthday is in {item.Key} days. ({item.Value.Bday.Date.ToShortDateString()})");

                }
            }
            await ReplyAsync(sb.ToString());
        }


        [Command("bday"), Description("Register your Bday by Mummybot"), RequireActiveBdayService]
        [RunMode(RunMode.Parallel)]
        public async Task Bday(SocketGuildUser user = null)
        {
            user = user ?? Context.User;
            var bdays = GuildConfig.Birthdays;
            if (bdays.Any(x => x.UserID == user.Id))
            {
                await ReplyAsync("mate i already know that bday, pls remove it first if its wrong or if you wanne unsub from bdays");
            }

            DateTime bday = new DateTime();

            await ReplyAsync("please tell me what dateformat your gonne give me (dmy/mdy).");
            var response1 = await InteractiveService.NextMessageAsync(Context, timeout: TimeSpan.FromMinutes(2));
            if (response1.Content != "" && response1.Content.ToLower() == "dmy")
            {
                var success = false;
                int tries = 0;
                do
                {
                    if (tries >= 3)
                        return;
                    try
                    {
                        await ReplyAsync("ok tell me the date");
                        var response2 = await InteractiveService.NextMessageAsync(Context, timeout: TimeSpan.FromMinutes(2));
                        bday = DateTime.ParseExact(response2.Content, "dd/MM/yyyy", CultureInfo.InvariantCulture.DateTimeFormat);
                    }
                    catch (FormatException)
                    {
                        await ReplyAsync("Parsing failed please try again (dmy format)");
                        tries++;
                    }
                } while (!success);


            }
            else if (response1.Content != "" && response1.Content.ToLower() == "mdy")
            {
                var success = false;
                int tries = 0;
                do
                {
                    if (tries >= 3)
                        return;

                    try
                    {
                        await ReplyAsync("ok tell me the date");
                        var response2 = await InteractiveService.NextMessageAsync(Context, timeout: TimeSpan.FromMinutes(2));
                        bday = DateTime.ParseExact(response2.Content, "MM/dd/yyyy", CultureInfo.InvariantCulture.DateTimeFormat);
                    }
                    catch (FormatException)
                    {
                        await ReplyAsync("Parsing failed please try again (mdy)");
                        tries++;
                    }
                } while (!success);

            }

            Birthday Bday = new Birthday
            {
                UserID = user.Id,
                Bday = bday,
            };
            GuildConfig.Birthdays.Add(Bday);
            var msg = await ReplyAsync($"Registered Birthday of {user.Username} on {Bday.Bday.ToString("dd/MM/yyyy")} (dmy).");
            await Task.Delay(5000);
            await Messages.DeleteMessageAsync(Context, msg);
        }







        [Command("bdayrole")]
        public async Task Setroleid(IRole channel)
        {
            GuildConfig.BdayroleID = channel.Id;
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("bdaychannel")]
        public async Task Setchannelid(ITextChannel channel)
        {
            GuildConfig.BdaychannelID = channel.Id;
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }
    }


}