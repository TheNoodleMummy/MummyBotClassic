using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class VoiceDeafUser
    {
        public Guild Guild { get; set; }
        public ulong Id { get; set; }
        public ulong GuildID { get; set; }

        public DateTimeOffset SetAtUTC { get; set; }
        public DateTimeOffset ExpiresAtUTC { get; set; }
        public ulong UserID { get; set; }
        public ulong ChannelID { get; set; }
    }
}
