using Discord;
using Qmmands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mummybot.Database.Models;
using Mummybot.Database;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;

namespace Mummybot.Services
{
    //TODO remake this so it works with the timer service
    [Service("Birthday Service",typeof(BirthdayService))]
    public class BirthdayService
    {
        private Timer T = null;
        [Inject]
        private GuildService GuildService;
        private DiscordSocketClient _client;
        private DBService DBService;
        private LogService Log;
        public BirthdayService(DiscordSocketClient discordSocketClient,DBService dBService,LogService logService)
        {
            T = new Timer(CheckBDays);

            _client = discordSocketClient;
            DBService = dBService;
            Log = logService;
        }

        
        public async void CheckBDays(object state = null)
        {
            foreach (var guiild in _client.Guilds)
            {
                var guild =  DBService.GetGuildUncached(guiild);
                Log.LogInformation($"checking bdays for {guiild.Name}",Enums.LogSource.BdayService);
                if (!guild.UsesBirthdays)
                {
                    Log.LogInformation($"{guiild.Name} does not use the bdays system skipping.",Enums.LogSource.BdayService);
                    continue; //if the guild doesnt use bdays leave it as is and go to next 
                }
                var bdays = guild.Birthdays.Where(x => x.Bday.Day == DateTime.UtcNow.Day && x.Bday.Month == DateTime.UtcNow.Month).ToList();
                Log.LogInformation($"Found {bdays.Count} bdays", Enums.LogSource.BdayService);
                List<CurrentBday> users = new List<CurrentBday>();
                StringBuilder text;
                if (bdays == null)
                    bdays = new List<Birthday>();

                //getting full user object for all people with a bday today
                if (bdays != null || bdays.Count != 0)
                {
                    foreach (Birthday bday in bdays)
                    {
                        users.Add(new CurrentBday(_client.GetGuild(guild.GuildID).GetUser(bday.UserID) as SocketGuildUser, bday.Bday, DateTime.Now.Year - bday.Bday.Year));
                    }
                }

              
                var role = guiild.GetRole(guild.BdayroleID);
                if (role == null)
                {
                    Log.LogError($"could not find the bday role in {guiild.Name}, exiting.",Enums.LogSource.BdayService);
                    continue;
                }
                foreach (var user in role.Members)
                {
                    await user.RemoveRoleAsync(role);
                    Log.LogInformation($"Removed Bday party role from {user}.", Enums.LogSource.BdayService);
                }
                //give the bday role
                if (bdays.Count != 0)
                {
                    foreach (var bday in users)
                    {
                        await bday.User.AddRoleAsync(role);
                        Log.LogInformation($"Gave Bday party role to {bday.User.Username}/{bday.User.Id}.", Enums.LogSource.BdayService);
                    }
                }


                //remove the bday role from those who dont have a bday today
                var channel = (_client.GetGuild(guild.GuildID).GetTextChannel(guild.BdaychannelID));
                

                if (channel != null)
                {
                    if (bdays.Count == 0)
                    {
                        Log.LogInformation("Nobody's Birthday today :(", Enums.LogSource.BdayService);
                        CalculateAndStart();
                        continue;
                    }
                    foreach (var bday in users)
                    {
                        
                        if (bdays.Count == 1)
                        {
                            text = new StringBuilder($"Today is the Birthday of {bday.User.Mention} and turns {bday.Age}.");
                        }
                        else
                        {
                            text = new StringBuilder($"Today is the Birthday of ");
                            for (int i = 0; i < users.Count; i++)
                            {
                                text.Append($", {users[i].User.Mention} turns {users[i].Age}");
                            }
                            text.Append(",");
                        }
                        await channel.SendMessageAsync(text.ToString()+ "\nHappy Birthday 🎂");
                    }
                }
            }
            CalculateAndStart();
        }

        public void CalculateAndStart()
        {
            TimeSpan untilMidnight = DateTime.Today.AddDays(1.0) - DateTime.UtcNow;
            T.Change(untilMidnight, TimeSpan.Zero);
        }
    }

    public class CurrentBday
    {
        public CurrentBday(SocketGuildUser user, DateTime bday, int turns)
        {
            this.User = user;
            Bday = bday;
            Age = turns;
        }

        public DateTime Bday { get; private set; }
        public SocketGuildUser User { get; private set; }
        public int Age { get; private set; }
    }
}