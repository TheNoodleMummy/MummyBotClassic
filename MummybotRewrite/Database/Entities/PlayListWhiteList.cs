using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class PlayListWhiteList
    {
        public Guild Guild { get; set; }
        public ulong Id { get; set; }
        public ulong GuildID { get; set; }

        public ulong UserId { get; set; }
        public ulong WhiteListedBy { get; set; }
        public DateTimeOffset WhiteListedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}
