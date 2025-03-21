﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Guild
    {
        public List<Prefixes> Prefixes { get; set; } = new List<Prefixes>();
        public List<Star> Stars { get; set; } = new List<Star>();
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Birthday> Birthdays { get; set; } = new List<Birthday>();

        public List<VoiceMutedUser> VoiceMutedUsers { get; set; } = new List<VoiceMutedUser>();
        public List<VoiceDeafUser> VoiceDeafenedUsers { get; set; } = new List<VoiceDeafUser>();

        public List<PlayListWhiteList> PlayListWhiteLists { get; set; } = new List<PlayListWhiteList>();

        public ulong GuildID { get; set; }

        public ulong HangManRoleId { get;  set; }
        public ulong HangManChannelID { get;  set; }
        public bool UsesHangman { get;  set; }

        public bool AutoQuotes { get; set; }

        public ulong StarboardChannelId { get; set; }
        public string StarboardEmote { get; set; } = "⭐";//this is a string not char becuase it allows for custom guild emotes these are not unicode andtherefor dont fit the char type

        public ulong BdayChannelId { get; set; }

        public bool UsesStarBoard { get; set; }
        public bool UsesReminders { get; set; }
        public bool UsesTags { get; set; }
        public bool UsesBirthdays { get; set; }
        public bool UsesMusic { get; set; }
        public bool UsesTrolls { get; set; }
        public bool AllowOffensiveCommands { get; set; } = false;
        public bool Allow18PlusCommands { get; set; } = false;
        public int Volume { get; set; }
        public int DefualtVolume { get; set; }
       
    }
}
