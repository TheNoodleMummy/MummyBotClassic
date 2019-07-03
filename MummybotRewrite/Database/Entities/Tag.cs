using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Tag
    {
        public ulong UserId { get; set; }
        public string Message { get; set; }
        public string Key { get; set; }

        public ulong Id { get; set; }

        public Guild Guild { get; set; }
        public ulong GuildID { get; set; }
    }
}
