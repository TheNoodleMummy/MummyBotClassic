using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Reminder
    {
        public Guild Guild { get; set;}
        public ulong Id { get; set; }
        public ulong GuildID { get; set; }

        public string Message { get; set; }
        public DateTimeOffset SetAtUTC { get; set; }
        public DateTimeOffset ExpiresAtUTC { get; set; }
        public ulong UserID { get; set; }
        public ulong ChannelID { get; set; }
        public string JumpUrl { get; set; }

        public override string ToString()
        {
            return $"{UserID} / {Message}";
        }
    }
}
