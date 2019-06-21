using Mummybot.Attributes;
using Mummybot.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Database.Models
{
    public class Guild : IRemoveable
    {
        [NotMapped]
        private readonly IRemoveableService _service;

        public Guild() { }
        public Guild(IRemoveableService service)
        {
            _service = service;
        }
        public Guild(Guild guild, IRemoveableService service)
        {
            GuildID = guild.GuildID;
            Prefixes = guild.Prefixes;
            Stars = guild.Stars;
            Reminders = guild.Reminders;
            Tags = guild.Tags;
            Birthdays = guild.Birthdays;
            BlackList = guild.BlackList;
            RoleBackups = guild.RoleBackups;
            IsBlackListed = guild.IsBlackListed;
            UsesBlackList = guild.UsesBlackList;
            UsesStarboard = guild.UsesStarboard;
            StarBoardChannelID = guild.StarBoardChannelID;
            UsesReminders = guild.UsesReminders;
            UsesBirthdays = guild.UsesBirthdays;
            BdaychannelID = guild.BdaychannelID;
            BdayroleID = guild.BdaychannelID;
            UsesVoiceTrolls = guild.UsesVoiceTrolls;
            UsesTags = guild.UsesTags;
            CanPlayHangman = guild.CanPlayHangman;
            HangmanChannelid = guild.HangmanChannelid;
            UnknownCommands = guild.UnknownCommands;
            AdvancedCommandErrors = guild.AdvancedCommandErrors;
            When = guild.When;
            _service = service;
        }

        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GuildID { get; set; }

        [GetCountList, PropName("Prefixes")]
        public List<string> Prefixes { get; set; } = new List<string>() { "!" };

        [GetCountList, PropName("Stared Messages")]
        public List<Star> Stars { get; set; } = new List<Star>();

        [GetCountList, PropName("Reminders")]
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();

        [GetCountList, PropName("User Tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [GetCountList, PropName("Registered Birthdays")]
        public List<Birthday> Birthdays { get; set; } = new List<Birthday>();

        [GetCountList, PropName("Users Blacklisted")]
        public List<BlackList> BlackList { get; set; } = new List<BlackList>();


        public List<RoleBackup> RoleBackups { get; set; } = new List<RoleBackup>();


        [OnOffBool, PropName("Is BlackListed")]
        public bool IsBlackListed { get; set; } = false;

        [OnOffBool, PropName("uses UserBlacklist")]
        public bool UsesBlackList { get; set; } = false;

        [OnOffBool, PropName("Uses Starboard")]
        public bool UsesStarboard { get; set; } = false;


        public ulong StarBoardChannelID { get; set; } = 0;

        [OnOffBool, PropName("Reminders")]
        public bool UsesReminders { get; set; } = false;

        [OnOffBool, PropName("Birthday Announcements")]
        public bool UsesBirthdays { get; set; } = false;


        public ulong BdaychannelID { get; set; } = 0;


        public ulong BdayroleID { get; set; } = 0;

        [OnOffBool, PropName("none playlist related voice commands")]
        public bool UsesVoiceTrolls { get; set; } = false;

        [OnOffBool, PropName("User Tags")]
        public bool UsesTags { get; set; } = false;

        [OnOffBool, PropName("Hangman (the game)")]
        public bool CanPlayHangman { get; set; } = false;


        public ulong HangmanChannelid { get; set; } = 0;

        [OnOffBool, PropName("get unknown command reply")]
        public bool UnknownCommands { get; set; } = false;

        [OnOffBool, PropName("get detailed errors (non program errors)")]
        public bool AdvancedCommandErrors { get; set; } = false;

        #endregion  
        public int Identifier { get; set; }/*memory shizzle id*/

        [NotMapped]
        public DateTime When { get; set; }/*when removed from cache (memory shizzle)*/

        public Task RemoveAsync()
       => _service.RemoveAsync(this);

        public string GetConfig()
        {

            var sb = new StringBuilder();
            sb.AppendLine("this guild has:");
            sb.Append(GetCounters());
            sb.AppendLine("and can use");
            sb.AppendLine(Getbools());

            return sb.ToString();
        }
        public string GetCounters()
        {
            var properties = GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(GetCountListAttribute), true).Length > 0).ToList();
            var sb = new StringBuilder();
            foreach (var property in properties)
            {
                var get = property.GetGetMethod();
                var value = get.Invoke(this, null);
                int count = 0;
                if (value is List<string> outstring)
                {
                    count = outstring.Count;
                }
                else if (value is List<object> outobj)
                {
                    count = outobj.Count;
                }
                var descattrib = property.GetCustomAttribute<PropNameAttribute>().Name;
                sb.AppendLine($"{count} {descattrib}");
            }


            return sb.ToString();
        }
        public string Getbools()
        {
            var properties = GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(OnOffBoolAttribute), true).Length > 0).ToList();

            var sb = new StringBuilder();
            foreach (var property in properties)
            {
                var descattrib = property.GetCustomAttribute<PropNameAttribute>().Name;
                var get = property.GetGetMethod();
                var value = (bool)get.Invoke(this, null);

                sb.AppendLine($"{descattrib}: {(value ? "yes" : "no")}");
            }
            return sb.ToString();
        }
    }
}
